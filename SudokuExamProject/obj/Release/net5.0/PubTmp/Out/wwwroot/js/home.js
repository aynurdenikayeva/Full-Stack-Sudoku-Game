
document.addEventListener("DOMContentLoaded", function () {
    const game = document.getElementById("sudokuGame");

    if (!game) return;

    const board = game.querySelector(".sudoku-board");
    const timerElement = document.getElementById("gameTimer");
    const messageElement = document.getElementById("gameMessage");
    const mistakesElement = document.getElementById("mistakesCount");
    const hintsElement = document.getElementById("hintsCount");
    const difficultyElement = document.getElementById("difficultyLabel");
    const completedMessage = document.getElementById("completedMessage");

    const hintUrl = game.dataset.hintUrl;
    const clearUrl = game.dataset.clearUrl;
    const undoUrl = game.dataset.undoUrl;
    const addUrl = game.dataset.addUrl;

    let selectedCell = null;
    let elapsedSeconds = 0;
    let timerInterval = null;
    let gameOver = false;

    // Antiforgery token
    function getToken() {
        const input = game.querySelector(
            'input[name="__RequestVerificationToken"]'
        );

        return input ? input.value : "";
    }

    // Vaxt sayğacı
    function updateTimer() {
        const minutes = Math.floor(elapsedSeconds / 60);
        const seconds = elapsedSeconds % 60;

        timerElement.textContent =
            String(minutes).padStart(2, "0") + ":" +
            String(seconds).padStart(2, "0");
    }

    function startTimer() {
        if (timerInterval !== null || gameOver) return;

        timerInterval = setInterval(function () {
            elapsedSeconds++;
            updateTimer();
        }, 1000);
    }

    function stopTimer() {
        if (timerInterval !== null) {
            clearInterval(timerInterval);
            timerInterval = null;
        }
    }

    function showMessage(message) {
        if (messageElement) {
            messageElement.textContent = message || "";
        }
    }

    function selectCell(cell) {
        if (!cell || cell.disabled || gameOver) return;

        if (selectedCell) {
            selectedCell.classList.remove("selected-cell");
        }

        selectedCell = cell;
        selectedCell.classList.add("selected-cell");
    }

    // Lövhədə xanaları seçmək
    board.addEventListener("click", function (event) {
        const cell = event.target.closest(".cell-input");

        if (cell) {
            selectCell(cell);
        }
    });

    board.addEventListener("focusin", function (event) {
        const cell = event.target.closest(".cell-input");

        if (cell) {
            selectCell(cell);
        }
    });

    function getSelectedCell() {
        if (!selectedCell || !board.contains(selectedCell)) {
            showMessage("Əvvəlcə dəyişdirmək istədiyin boş xananı seç.");
            return null;
        }

        return selectedCell;
    }

    // Səhv və ipucu saylarını yenilə
    function updateCounters(result) {
        if (result.mistakes !== undefined) {
            mistakesElement.textContent = result.mistakes;
        }

        if (result.hintsUsed !== undefined) {
            hintsElement.textContent = result.hintsUsed;
        }

        if (result.difficulty && difficultyElement) {
            difficultyElement.textContent = result.difficulty;
        }
    }

    // Serverdən gələn Sudoku lövhəsini göstər
    function updateBoard(result) {
        if (!result.board) return;

        board.querySelectorAll(".cell-input").forEach(function (cell) {
            const row = Number(cell.dataset.row);
            const col = Number(cell.dataset.col);
            const value = result.board[row][col];

            cell.value = value === 0 ? "" : String(value);
        });

        // Tamamlanmış və ya uduzulmuş oyunu dayandır
        if (result.completed || result.lost) {
            gameOver = true;
            stopTimer();

            board.querySelectorAll(".cell-input").forEach(function (cell) {
                cell.disabled = true;
            });

            document.querySelectorAll(
                "#hintButton, #clearButton, #undoButton, .number-button"
            ).forEach(function (button) {
                button.disabled = true;
            });

            if (completedMessage && result.completed) {
                completedMessage.hidden = false;
            }
        }
    }

    // Controller-ə POST sorğusu göndər
    async function sendRequest(url, data = {}) {
        if (gameOver) {
            showMessage("Oyun bitib. Yeni oyun başlat.");
            return;
        }

        const body = new URLSearchParams();
        body.append("__RequestVerificationToken", getToken());

        Object.entries(data).forEach(function ([key, value]) {
            body.append(key, value);
        });

        try {
            const response = await fetch(url, {
                method: "POST",
                headers: {
                    "Content-Type": "application/x-www-form-urlencoded",
                    "X-Requested-With": "XMLHttpRequest"
                },
                body: body.toString()
            });

            let result;

            try {
                result = await response.json();
            } catch {
                showMessage("Serverdən düzgün cavab alınmadı.");
                return;
            }

            if (!response.ok) {
                showMessage(result.message || "Əməliyyat alınmadı.");
                return;
            }

            showMessage(result.message);
            updateCounters(result);
            updateBoard(result);

        } catch (error) {
            console.error("Sudoku sorğusu:", error);
            showMessage("Serverlə əlaqə qurulmadı. Yenidən cəhd et.");
        }
    }

    // İpucu
    document.getElementById("hintButton").addEventListener("click", function () {
        const cell = getSelectedCell();
        if (!cell) return;

        sendRequest(hintUrl, {
            row: cell.dataset.row,
            col: cell.dataset.col
        });
    });

    // Təmizlə
    document.getElementById("clearButton").addEventListener("click", function () {
        const cell = getSelectedCell();
        if (!cell) return;

        sendRequest(clearUrl, {
            row: cell.dataset.row,
            col: cell.dataset.col
        });
    });

    // Geri qaytar
    document.getElementById("undoButton").addEventListener("click", function () {
        sendRequest(undoUrl);
    });

    // Rəqəm düymələri
    document.querySelectorAll(".number-button").forEach(function (button) {
        button.addEventListener("click", function () {
            const cell = getSelectedCell();
            if (!cell) return;

            submitNumber(cell, button.dataset.number);
        });
    });

    // Klaviatura ilə rəqəm daxil etmək
    board.addEventListener("input", function (event) {
        const cell = event.target.closest(".cell-input");
        if (!cell || gameOver) return;

        selectCell(cell);

        const value = cell.value.trim();

        if (value === "") return;

        if (!/^[1-9]$/.test(value)) {
            cell.value = "";
            showMessage("Yalnız 1-dən 9-a qədər rəqəm daxil et.");
            return;
        }

        submitNumber(cell, value);
    });

    // Rəqəmi serverə göndər
    async function submitNumber(cell, number) {
        if (!/^[1-9]$/.test(String(number))) {
            showMessage("1-dən 9-a qədər rəqəm seç.");
            return;
        }

        await sendRequest(addUrl, {
            row: cell.dataset.row,
            col: cell.dataset.col,
            number: number
        });
    }

    // Yeni oyun forması adi POST ilə səhifəni yeniləyir.
    // Buna görə yeni oyun başlayanda vaxt yenidən 00:00 olur.
    startTimer();
});


document.addEventListener("DOMContentLoaded", function () {
    const game = document.getElementById("sudokuGame");
    if (!game) return;

    const board = game.querySelector(".sudoku-board");
    if (!board) return;

    // Rəqəmləri sayan və 9-a çatan düymələri söndürən müstəqil funksiya
    function checkAndDisableCompletedNumbers() {
        const numberButtons = document.querySelectorAll(".number-button");
        if (numberButtons.length === 0) return;

        // 1-dən 9-a qədər sayğac obyekti
        const counts = { "1": 0, "2": 0, "3": 0, "4": 0, "5": 0, "6": 0, "7": 0, "8": 0, "9": 0 };

        // Lövhədəki bütün td-ləri gəzib dəyərləri oxuyuruq
        board.querySelectorAll("td").forEach(function (td) {
            const input = td.querySelector(".cell-input");
            const fixed = td.querySelector(".fixed-number");
            let val = "";

            if (input) {
                val = input.value.trim();
            } else if (fixed) {
                val = fixed.textContent.trim();
            }

            if (val >= "1" && val <= "9") {
                counts[val]++;
            }
        });

        // Düymələrin vəziyyətini yeniləyirik
        numberButtons.forEach(function (btn) {
            const btnNum = btn.getAttribute("data-number");
            if (counts[btnNum] >= 9) {
                btn.disabled = true;
                btn.classList.add("completed");
            } else {
                // Əgər oyun hələ bitməyibsə, düyməni yenidən aktiv et
                const timerElement = document.getElementById("gameTimer");
                if (timerElement && !btn.closest('#sudokuGame[data-game-over="true"]')) {
                    btn.disabled = false;
                }
                btn.classList.remove("completed");
            }
        });
    }

    // 1. Səhifə ilk dəfə açılanda mövcud rəqəmləri say
    checkAndDisableCompletedNumbers();

    // 2. Sizin mövcud Fetch/AJAX kodunuz cədvəli yenilədikdə bunu tutmaq üçün Observer quraşdırırıq
    const observer = new MutationObserver(function (mutations) {
        checkAndDisableCompletedNumbers();
    });

    // Cədvəlin daxilindəki dəyişiklikləri (input value, disabled və s.) izləyirik
    observer.observe(board, {
        childList: true,
        subtree: true,
        attributes: true
    });
});
