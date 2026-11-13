/* =====================================================
   SUDOKUGAME — LÖVHƏ İNTERAKSİYASI
   Fayl: wwwroot/js/sudoku-board.js

   - Sətir, sütun, seçilmiş xana və eyni rəqəmlərin vurğulanması
   - Seçimin səhifə yenilənəndən sonra bərpası (sessionStorage)
   - Tab ilə keçid, ox düymələri ilə naviqasiya
   - Xanada yalnız 1-9 qəbul edilir, mövcud rəqəmin üstünə yazmaq olur
   - Sağdakı rəqəm düymələri seçilmiş xanaya rəqəm yazır
   ===================================================== */

(function () {
    'use strict';

    if (window.__sudokuBoardInit) return;
    window.__sudokuBoardInit = true;

    document.addEventListener('DOMContentLoaded', () => {
        const table = document.querySelector('.sudoku-board');
        if (!table) return;

        const grid = Array.from(table.querySelectorAll('tr')).map(tr => Array.from(tr.cells));
        const allCells = grid.flat();

        const STORAGE_KEY = 'sudoku-selected:' + location.pathname;
        const STATE_CLASSES = ['highlight', 'selected', 'same-number'];

        let current = null;

        const rowOf = cell => grid.findIndex(row => row.includes(cell));
        const inputOf = cell => (cell ? cell.querySelector('.cell-input') : null);

        const valueOf = cell => {
            const input = inputOf(cell);
            const fixed = cell.querySelector('.fixed-number');
            return (input ? input.value : fixed ? fixed.textContent : '').trim();
        };

        function select(cell, save = true) {
            if (!cell) return;
            current = cell;

            const rowIndex = rowOf(cell);
            const colIndex = cell.cellIndex;

            allCells.forEach(c => c.classList.remove(...STATE_CLASSES));

            grid[rowIndex].forEach(c => c.classList.add('highlight'));
            grid.forEach(row => row[colIndex] && row[colIndex].classList.add('highlight'));

            cell.classList.add('selected');

            const value = valueOf(cell);
            if (value !== '') {
                allCells.forEach(c => {
                    if (valueOf(c) === value) c.classList.add('same-number');
                });
            }

            if (save) {
                try { sessionStorage.setItem(STORAGE_KEY, rowIndex + ',' + colIndex); } catch (e) { }
            }
        }

        // Siçan / toxunuş (sabit rəqəmli xanalar fokus almır)
        table.addEventListener('click', e => select(e.target.closest('td')));

        // Tab ilə keçid; fokusda mövcud rəqəm seçilir ki, yazı onu əvəz etsin
        table.addEventListener('focusin', e => {
            select(e.target.closest('td'));
            if (e.target.classList.contains('cell-input')) {
                const input = e.target;
                setTimeout(() => input.select(), 0);
            }
        });

        // Yalnız 1-9; maxlength olmadığı üçün yeni rəqəm köhnəni əvəz edir
        table.addEventListener('input', e => {
            if (e.target.classList.contains('cell-input')) {
                e.target.value = e.target.value.replace(/[^1-9]/g, '').slice(-1);
            }
            select(current, false);
        });

        // Ox düymələri ilə naviqasiya
        table.addEventListener('keydown', e => {
            const move = {
                ArrowUp: [-1, 0],
                ArrowDown: [1, 0],
                ArrowLeft: [0, -1],
                ArrowRight: [0, 1]
            }[e.key];

            if (!move || !current) return;

            const next = grid[rowOf(current) + move[0]]?.[current.cellIndex + move[1]];
            if (!next) return;

            e.preventDefault();
            select(next);
            const input = inputOf(next);
            if (input) input.focus();
        });

        // Sağdakı rəqəm düymələri
        const keypad = document.querySelector('.number-keypad');
        if (keypad) {
            // Düyməyə basanda xana fokusu itməsin
            keypad.addEventListener('mousedown', e => {
                if (e.target.closest('.number-button')) e.preventDefault();
            });

            keypad.addEventListener('click', e => {
                const button = e.target.closest('.number-button');
                if (!button) return;

                const input = inputOf(current);
                if (!input || input.disabled) return;

                input.value = button.dataset.number;
                input.focus();

                // home.js-in "input" dinləyicisi də işləsin deyə hadisə göndərilir
                input.dispatchEvent(new Event('input', { bubbles: true }));
            });
        }

        // Səhifə yenilənəndən sonra seçimi bərpa et
        try {
            const saved = sessionStorage.getItem(STORAGE_KEY);
            if (saved) {
                const [r, c] = saved.split(',').map(Number);
                const cell = grid[r]?.[c];
                select(cell, false);
                const input = inputOf(cell);
                if (input) input.focus({ preventScroll: true });
            }
        } catch (e) { }

        // Yeni oyunda köhnə seçimi təmizlə
        document.querySelector('.new-game-form')?.addEventListener('submit', () => {
            try { sessionStorage.removeItem(STORAGE_KEY); } catch (e) { }
        });

        // home.js üçün
        window.getSelectedSudokuCell = () => current;
        window.refreshSudokuHighlight = () => select(current, false);
    });
})();