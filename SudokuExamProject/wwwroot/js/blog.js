document.addEventListener("DOMContentLoaded", function () {

    // HTML elementlərini seçirik
    const blogGrid = document.getElementById("blogGrid");
    const blogCards = Array.from(
        blogGrid.querySelectorAll(".blog-card")
    );

    const searchInput = document.getElementById("blogSearch");
    const filterButtons = document.querySelectorAll(".filter-btn");

    const resultsCount = document.getElementById("resultsCount");
    const blogEmpty = document.getElementById("blogEmpty");
    const resetButton = document.getElementById("resetBlogFilters");

    const pagination = document.getElementById("blogPagination");
    const pageNumbers = document.getElementById("pageNumbers");
    const prevButton = document.getElementById("prevPage");
    const nextButton = document.getElementById("nextPage");

    // Bir səhifədə göstəriləcək məqalələrin sayı
    const pageSize = 6;

    // Cari səhifə və seçilmiş kateqoriya
    let currentPage = 1;
    let selectedCategory = "all";

    // Axtarışa və kateqoriyaya uyğun məqalələri tapırıq
    function getFilteredArticles() {
        const searchText = searchInput.value
            .trim()
            .toLocaleLowerCase("az");

        return blogCards.filter(function (card) {
            const category = (
                card.dataset.category || ""
            ).toLocaleLowerCase("az");

            const title = (
                card.dataset.title || ""
            ).toLocaleLowerCase("az");

            const description = (
                card.dataset.description || ""
            ).toLocaleLowerCase("az");

            const matchesCategory =
                selectedCategory === "all" ||
                category === selectedCategory.toLocaleLowerCase("az");

            const matchesSearch =
                title.includes(searchText) ||
                description.includes(searchText) ||
                category.includes(searchText);

            return matchesCategory && matchesSearch;
        });
    }

    // Məqalələri və səhifələməni ekranda göstəririk
    function renderBlog() {
        const filteredArticles = getFilteredArticles();

        const totalArticles = filteredArticles.length;

        const totalPages = Math.ceil(totalArticles / pageSize);

        // Cari səhifənin düzgün aralıqda qalmasını təmin edirik
        if (totalPages === 0) {
            currentPage = 1;
        } else if (currentPage > totalPages) {
            currentPage = totalPages;
        }

        // Hansı məqalələrin göstəriləcəyini hesablayırıq
        const startIndex = (currentPage - 1) * pageSize;
        const endIndex = Math.min(
            startIndex + pageSize,
            totalArticles
        );

        const visibleArticles = filteredArticles.slice(
            startIndex,
            endIndex
        );

        // Əvvəlcə bütün məqalələri gizlədirik
        blogCards.forEach(function (card) {
            card.hidden = true;
        });

        // Yalnız cari səhifəyə aid 6 məqaləni göstəririk
        visibleArticles.forEach(function (card) {
            card.hidden = false;
        });

        // Nəticə sayını yeniləyirik
        if (totalArticles === 0) {
            resultsCount.textContent = "Məqalə tapılmadı";
        } else {
            resultsCount.textContent =
                `${startIndex + 1}–${endIndex} / ` +
                `${totalArticles} məqalə göstərilir`;
        }

        // Məqalə tapılmadıqda boş nəticə mesajı
        blogEmpty.hidden = totalArticles !== 0;

        // Səhifələmə yalnız birdən çox səhifə olduqda görünür
        pagination.hidden = totalPages <= 1;

        // Previous düyməsinin vəziyyəti
        prevButton.disabled =
            currentPage === 1 || totalPages === 0;

        // Next düyməsinin vəziyyəti
        nextButton.disabled =
            currentPage === totalPages || totalPages === 0;

        // Səhifə nömrələrini yaradırıq
        pageNumbers.innerHTML = "";

        for (let page = 1; page <= totalPages; page++) {
            const button = document.createElement("button");

            button.type = "button";
            button.className = "page-button";
            button.textContent = page;
            button.setAttribute(
                "aria-label",
                `${page}-ci səhifə`
            );

            if (page === currentPage) {
                button.classList.add("active");
                button.setAttribute("aria-current", "page");
            }

            button.addEventListener("click", function () {
                currentPage = page;
                renderBlog();
                scrollToArticles();
            });

            pageNumbers.appendChild(button);
        }
    }

    // Məqalələr bölməsinə yumşaq keçid
    function scrollToArticles() {
        document.getElementById("blog-articles")
            .scrollIntoView({
                behavior: "smooth",
                block: "start"
            });
    }

    // Kateqoriya seçimi
    filterButtons.forEach(function (button) {
        button.addEventListener("click", function () {
            filterButtons.forEach(function (item) {
                item.classList.remove("active");
            });

            button.classList.add("active");

            selectedCategory =
                button.dataset.category || "all";

            currentPage = 1;

            renderBlog();
        });
    });

    // Axtarış zamanı nəticələri yeniləyirik
    searchInput.addEventListener("input", function () {
        currentPage = 1;
        renderBlog();
    });

    // Əvvəlki səhifə
    prevButton.addEventListener("click", function () {
        if (currentPage > 1) {
            currentPage--;
            renderBlog();
            scrollToArticles();
        }
    });

    // Növbəti səhifə
    nextButton.addEventListener("click", function () {
        const totalPages = Math.ceil(
            getFilteredArticles().length / pageSize
        );

        if (currentPage < totalPages) {
            currentPage++;
            renderBlog();
            scrollToArticles();
        }
    });

    // Bütün filtrləri sıfırlayırıq
    resetButton.addEventListener("click", function () {
        searchInput.value = "";
        selectedCategory = "all";
        currentPage = 1;

        filterButtons.forEach(function (button) {
            button.classList.toggle(
                "active",
                button.dataset.category === "all"
            );
        });

        renderBlog();
        scrollToArticles();
    });

    // Səhifə ilk dəfə açıldıqda işə düşür
    renderBlog();

});