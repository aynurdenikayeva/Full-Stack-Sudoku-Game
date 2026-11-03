
document.addEventListener("DOMContentLoaded", function () {

    // ========================================
    // 1. MOBİL MENYU
    // ========================================

    const menuToggle = document.getElementById("menuToggle");
    const siteNav = document.getElementById("siteNav");

    if (menuToggle && siteNav) {

        function openMenu() {
            siteNav.classList.add("is-open");
            menuToggle.setAttribute("aria-expanded", "true");
            menuToggle.setAttribute("aria-label", "Menyunu bağla");

            const icon = menuToggle.querySelector("i");

            if (icon) {
                icon.classList.remove("fa-bars");
                icon.classList.add("fa-times");
            }
        }

        function closeMenu() {
            siteNav.classList.remove("is-open");
            menuToggle.setAttribute("aria-expanded", "false");
            menuToggle.setAttribute("aria-label", "Menyunu aç");

            const icon = menuToggle.querySelector("i");

            if (icon) {
                icon.classList.remove("fa-times");
                icon.classList.add("fa-bars");
            }
        }

        menuToggle.addEventListener("click", function () {
            if (siteNav.classList.contains("is-open")) {
                closeMenu();
            } else {
                openMenu();
            }
        });

        // Linkə klik etdikdə mobil menyunu bağla
        siteNav.querySelectorAll("a").forEach(function (link) {
            link.addEventListener("click", function () {
                if (window.innerWidth <= 850) {
                    closeMenu();
                }
            });
        });

        // Escape düyməsi ilə menyunu bağla
        document.addEventListener("keydown", function (event) {
            if (event.key === "Escape") {
                closeMenu();
            }
        });

        // Ekran ölçüsü dəyişdikdə menyunu yenilə
        window.addEventListener("resize", function () {
            if (window.innerWidth > 850) {
                closeMenu();
            }
        });
    }


    // ========================================
    // 2. DROPDOWN MENYULAR
    // ========================================

    document.querySelectorAll(".dropdown-toggle").forEach(function (button) {

        button.addEventListener("click", function (event) {
            event.preventDefault();

            const dropdown = button.closest(".nav-dropdown");

            if (!dropdown) return;

            const isOpen = dropdown.classList.contains("open");

            // Digər açıq dropdown-ları bağla
            document.querySelectorAll(".nav-dropdown.open").forEach(function (item) {
                if (item !== dropdown) {
                    item.classList.remove("open");

                    const otherButton = item.querySelector(".dropdown-toggle");

                    if (otherButton) {
                        otherButton.setAttribute("aria-expanded", "false");
                    }
                }
            });

            // Cari dropdown-u aç və ya bağla
            dropdown.classList.toggle("open", !isOpen);

            button.setAttribute("aria-expanded", String(!isOpen));
        });
    });


    // Dropdown xaricinə klik etdikdə bağla
    document.addEventListener("click", function (event) {
        if (!event.target.closest(".nav-dropdown")) {

            document.querySelectorAll(".nav-dropdown.open").forEach(function (dropdown) {
                dropdown.classList.remove("open");

                const button = dropdown.querySelector(".dropdown-toggle");

                if (button) {
                    button.setAttribute("aria-expanded", "false");
                }
            });
        }
    });


    // ========================================
    // 3. BACK TO TOP
    // ========================================

    const backToTop = document.getElementById("backToTop");

    if (backToTop) {

        function toggleBackToTop() {
            if (window.scrollY > 300) {
                backToTop.classList.add("show");
            } else {
                backToTop.classList.remove("show");
            }
        }

        window.addEventListener("scroll", toggleBackToTop, {
            passive: true
        });

        backToTop.addEventListener("click", function (event) {
            event.preventDefault();

            window.scrollTo({
                top: 0,
                behavior: "smooth"
            });
        });

        toggleBackToTop();
    }

});