// EventHub JavaScript functionality
document.addEventListener('DOMContentLoaded', function () {
    // Initialize favorites from localStorage
    let favorites = JSON.parse(localStorage.getItem('eventFavorites')) || [];

    // Initialize UI
    initializeFavorites();
    updateFavoritesDisplay();

    // Event listeners
    setupEventListeners();

    function setupEventListeners() {
        // Favorite buttons
        document.querySelectorAll('.favorite-btn').forEach(btn => {
            btn.addEventListener('click', handleFavoriteToggle);
        });

        // Show more/less buttons
        const showMoreBtn = document.getElementById('showMoreBtn');
        const showLessBtn = document.getElementById('showLessBtn');

        if (showMoreBtn) {
            showMoreBtn.addEventListener('click', showAllEvents);
        }

        if (showLessBtn) {
            showLessBtn.addEventListener('click', showLessEvents);
        }

        // Purchase buttons
        document.querySelectorAll('.purchase-btn').forEach(btn => {
            btn.addEventListener('click', handlePurchase);
        });

        // Details buttons
        document.querySelectorAll('.elegant-btn-secondary').forEach(btn => {
            btn.addEventListener('click', handleDetails);
        });

        // Favorites button in footer
        const favoritesBtn = document.getElementById('favoritesBtn');
        if (favoritesBtn) {
            favoritesBtn.addEventListener('click', handleViewFavorites);
        }
    }

    function initializeFavorites() {
        favorites.forEach(eventId => {
            const btn = document.querySelector(`[data-event-id="${eventId}"]`);
            if (btn) {
                btn.classList.add('favorited');
                const icon = btn.querySelector('.btn-icon');
                if (icon) {
                    icon.classList.add('filled');
                }
            }
        });
    }

    function handleFavoriteToggle(e) {
        e.preventDefault();
        e.stopPropagation();

        const btn = e.currentTarget;
        const eventId = btn.getAttribute('data-event-id');
        const isFavorited = btn.classList.contains('favorited');
        const savedCountElement = btn.closest('.elegant-card').querySelector('.saved-count');

        if (isFavorited) {
            // Remove from favorites
            favorites = favorites.filter(id => id !== eventId);
            btn.classList.remove('favorited');
            const icon = btn.querySelector('.btn-icon');
            if (icon) {
                icon.classList.remove('filled');
            }

            // Decrease count
            if (savedCountElement) {
                const currentCount = parseInt(savedCountElement.textContent);
                savedCountElement.textContent = currentCount - 1;
            }
        } else {
            // Add to favorites
            favorites.push(eventId);
            btn.classList.add('favorited');
            const icon = btn.querySelector('.btn-icon');
            if (icon) {
                icon.classList.add('filled');
            }

            // Increase count
            if (savedCountElement) {
                const currentCount = parseInt(savedCountElement.textContent);
                savedCountElement.textContent = currentCount + 1;
            }
        }

        // Save to localStorage
        localStorage.setItem('eventFavorites', JSON.stringify(favorites));

        // Update favorites display
        updateFavoritesDisplay();

        // Add animation effect
        btn.style.transform = 'scale(1.2)';
        setTimeout(() => {
            btn.style.transform = '';
        }, 200);
    }

    function updateFavoritesDisplay() {
        const favoritesText = document.getElementById('favoritesText');
        const favoritesBtn = document.getElementById('favoritesBtn');
        const count = favorites.length;

        if (favoritesText) {
            if (count > 0) {
                favoritesText.textContent = `You have ${count} favorite event${count !== 1 ? 's' : ''} saved`;
            } else {
                favoritesText.textContent = 'Start saving events to see your favorites here';
            }
        }

        if (favoritesBtn) {
            const btnText = favoritesBtn.querySelector('span:not(.btn-icon)');
            if (count > 0) {
                favoritesBtn.innerHTML = '<i class="fas fa-heart btn-icon me-2"></i>View Favorites';
            } else {
                favoritesBtn.innerHTML = '<i class="fas fa-heart btn-icon me-2"></i>Find Events';
            }
        }
    }

    function showAllEvents() {
        const hiddenCards = document.querySelectorAll('.event-card.d-none');
        const showMoreBtn = document.getElementById('showMoreBtn');
        const showLessBtn = document.getElementById('showLessBtn');

        hiddenCards.forEach(card => {
            card.classList.remove('d-none');
            // Add entrance animation
            card.style.opacity = '0';
            card.style.transform = 'translateY(20px)';
            setTimeout(() => {
                card.style.transition = 'all 0.5s ease';
                card.style.opacity = '1';
                card.style.transform = 'translateY(0)';
            }, 100);
        });

        if (showMoreBtn) showMoreBtn.classList.add('d-none');
        if (showLessBtn) showLessBtn.classList.remove('d-none');

        // Re-attach event listeners to newly shown cards
        hiddenCards.forEach(card => {
            const favoriteBtn = card.querySelector('.favorite-btn');
            const purchaseBtn = card.querySelector('.purchase-btn');
            const detailsBtn = card.querySelector('.elegant-btn-secondary');

            if (favoriteBtn) {
                favoriteBtn.addEventListener('click', handleFavoriteToggle);
            }
            if (purchaseBtn) {
                purchaseBtn.addEventListener('click', handlePurchase);
            }
            if (detailsBtn) {
                detailsBtn.addEventListener('click', handleDetails);
            }
        });
    }

    function showLessEvents() {
        const allCards = document.querySelectorAll('.event-card');
        const showMoreBtn = document.getElementById('showMoreBtn');
        const showLessBtn = document.getElementById('showLessBtn');

        // Hide cards after the first 3
        allCards.forEach((card, index) => {
            if (index >= 3) {
                card.style.transition = 'all 0.3s ease';
                card.style.opacity = '0';
                card.style.transform = 'translateY(-20px)';
                setTimeout(() => {
                    card.classList.add('d-none');
                    card.style.opacity = '';
                    card.style.transform = '';
                    card.style.transition = '';
                }, 300);
            }
        });

        if (showMoreBtn) showMoreBtn.classList.remove('d-none');
        if (showLessBtn) showLessBtn.classList.add('d-none');

        // Scroll to top of events section
        document.querySelector('.events-grid').scrollIntoView({
            behavior: 'smooth',
            block: 'start'
        });
    }

    function handlePurchase(e) {
        e.preventDefault();
        const card = e.currentTarget.closest('.elegant-card');
        const eventTitle = card.querySelector('.card-title').textContent;
        const price = card.querySelector('.price-amount').textContent;

        // Add purchase animation
        const btn = e.currentTarget;
        btn.style.transform = 'scale(0.95)';
        setTimeout(() => {
            btn.style.transform = '';
        }, 150);

        alert(`Purchase initiated for: ${eventTitle}\nPrice: $${price}\n\nThis would redirect to payment processing...`);

    }

    function handleDetails(e) {
        e.preventDefault();
        const card = e.currentTarget.closest('.elegant-card');
        const eventTitle = card.querySelector('.card-title').textContent;

        alert(`Showing details for: ${eventTitle}\n\nThis would show a detailed view or modal...`);

    }

    function handleViewFavorites(e) {
        e.preventDefault();

        if (favorites.length === 0) {
            alert('You haven\'t saved any favorite events yet!\n\nClick the heart icon on any event to add it to your favorites.');
            return;
        }
        const favoriteEvents = favorites.map(id => {
            const card = document.querySelector(`[data-event-id="${id}"]`).closest('.elegant-card');
            return card.querySelector('.card-title').textContent;
        }).join('\n• ');

        alert(`Your Favorite Events:\n\n• ${favoriteEvents}\n\nThis would redirect to your favorites page...`);

    }
    $(function () {
        const token = $('#antiForgeryForm input[name="__RequestVerificationToken"]').val();

        $.ajaxSetup({
            beforeSend: function (xhr, settings) {
                if (settings.type === "POST") {
                    // Attach token as header
                    xhr.setRequestHeader("RequestVerificationToken", token);

                    // Also attach token in form body if data is an object
                    if (typeof settings.data === "object") {
                        settings.data.__RequestVerificationToken = token;
                    }
                }
            }
        });

        document.querySelectorAll('a[href^="#"]').forEach(anchor => {
            anchor.addEventListener('click', function (e) {
                e.preventDefault();
                const target = document.querySelector(this.getAttribute('href'));
                if (target) {
                    target.scrollIntoView({
                        behavior: 'smooth',
                        block: 'start'
                    });
                }
            });
        });

        document.querySelectorAll('.card-image').forEach(img => {
            img.addEventListener('load', function () {
                this.style.opacity = '0';
                this.style.transition = 'opacity 0.5s ease';
                setTimeout(() => {
                    this.style.opacity = '1';
                }, 100);
            });
        });
    });
})
