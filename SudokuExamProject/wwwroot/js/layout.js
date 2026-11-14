
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
            const isOpen = siteNav.classList.contains("is-open");

            if (isOpen) {
                closeMenu();
            } else {
                openMenu();
            }
        });

        siteNav.querySelectorAll("a").forEach(function (link) {
            link.addEventListener("click", function () {
                if (window.innerWidth <= 850) {
                    closeMenu();
                }
            });
        });

        document.addEventListener("keydown", function (event) {
            if (event.key === "Escape") {
                closeMenu();
            }
        });

        window.addEventListener("resize", function () {
            if (window.innerWidth > 850) {
                closeMenu();
            }
        });
    }


    // ========================================
    // 2. İSTİFADƏÇİ PROFİL DROPDOWN
    // ========================================

    const profileButton = document.getElementById("profileDropdownBtn");
    const profileMenu = document.getElementById("profileDropdownMenu");



    if (profileButton && profileMenu) {

        function closeProfileDropdown() {
            profileMenu.classList.remove("show");
            profileButton.setAttribute("aria-expanded", "false");
        }

        profileButton.addEventListener("click", function (event) {
            event.stopPropagation();

            const isOpen = profileMenu.classList.contains("show");

            if (isOpen) {
                closeProfileDropdown();
            } else {
                profileMenu.classList.add("show");
                profileButton.setAttribute("aria-expanded", "true");
            }
        });

        // Menyudan kənara klik etdikdə bağlanır
        document.addEventListener("click", function (event) {
            if (!profileButton.contains(event.target) &&
                !profileMenu.contains(event.target)) {
                closeProfileDropdown();
            }
        });

        // Escape düyməsi ilə bağlanır
        document.addEventListener("keydown", function (event) {
            if (event.key === "Escape") {
                closeProfileDropdown();
            }
        });

        // Profil menyusundakı keçidə klik etdikdə bağlanır
        profileMenu.querySelectorAll("a").forEach(function (link) {
            link.addEventListener("click", closeProfileDropdown);
        });
    }


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

        backToTop.addEventListener("click", function () {
            window.scrollTo({
                top: 0,
                behavior: "smooth"
            });
        });

        toggleBackToTop();
    }

});
