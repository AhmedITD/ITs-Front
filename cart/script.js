// Sample products database
const products = [
    {
        id: 1,
        name: 'Laptop Pro',
        price: 1299.99,
        emoji: '💻',
        description: 'High-performance laptop for professionals',
        rating: 4.8
    },
    {
        id: 2,
        name: 'Wireless Headphones',
        price: 199.99,
        emoji: '🎧',
        description: 'Premium sound quality with noise cancellation',
        rating: 4.6
    },
    {
        id: 3,
        name: 'Smart Watch',
        price: 299.99,
        emoji: '⌚',
        description: 'Track your fitness and stay connected',
        rating: 4.5
    },
    {
        id: 4,
        name: 'USB-C Cable',
        price: 19.99,
        emoji: '🔌',
        description: 'Fast charging and data transfer',
        rating: 4.9
    },
    {
        id: 5,
        name: 'Power Bank',
        price: 49.99,
        emoji: '🔋',
        description: '20000mAh portable charger',
        rating: 4.7
    },
    {
        id: 6,
        name: 'Phone Case',
        price: 29.99,
        emoji: '📱',
        description: 'Durable protection with style',
        rating: 4.4
    },
    {
        id: 7,
        name: 'Mechanical Keyboard',
        price: 149.99,
        emoji: '⌨️',
        description: 'RGB backlit gaming keyboard',
        rating: 4.8
    },
    {
        id: 8,
        name: 'Wireless Mouse',
        price: 79.99,
        emoji: '🖱️',
        description: 'Precision tracking and long battery life',
        rating: 4.6
    },
    {
        id: 9,
        name: 'Monitor Stand',
        price: 89.99,
        emoji: '🖥️',
        description: 'Adjustable dual monitor stand',
        rating: 4.5
    },
    {
        id: 10,
        name: 'Desk Lamp',
        price: 59.99,
        emoji: '💡',
        description: 'Dimmable LED desk lamp with USB charging',
        rating: 4.7
    },
    {
        id: 11,
        name: 'Webcam',
        price: 99.99,
        emoji: '📹',
        description: '4K resolution for streaming and calls',
        rating: 4.6
    },
    {
        id: 12,
        name: 'HDMI Cable',
        price: 14.99,
        emoji: '🎬',
        description: '4K 60Hz HDMI 2.1 cable',
        rating: 4.8
    }
];

let cart = JSON.parse(localStorage.getItem('cart')) || [];
let orderHistory = JSON.parse(localStorage.getItem('orderHistory')) || [];

// Load theme preference
const isDarkMode = JSON.parse(localStorage.getItem('darkMode') !== 'false');
if (!isDarkMode) {
    document.body.classList.add('light-mode');
}

function playSound() {
    try {
        const audioContext = new (window.AudioContext || window.webkitAudioContext)();
        const oscillator = audioContext.createOscillator();
        const gain = audioContext.createGain();
        
        oscillator.connect(gain);
        gain.connect(audioContext.destination);
        
        oscillator.frequency.value = 800;
        oscillator.type = 'sine';
        
        gain.gain.setValueAtTime(0.3, audioContext.currentTime);
        gain.gain.exponentialRampToValueAtTime(0.01, audioContext.currentTime + 0.1);
        
        oscillator.start(audioContext.currentTime);
        oscillator.stop(audioContext.currentTime + 0.1);
    } catch (e) {
        // Audio context not supported, silently fail
    }
}

// Initialize products display
function initializeProducts() {
    const productsGrid = document.getElementById('productsGrid');
    productsGrid.innerHTML = '';

    products.forEach(product => {
        const productCard = document.createElement('div');
        productCard.className = 'product-card';
        productCard.innerHTML = `
            <div class="product-image">${product.emoji}</div>
            <div class="product-name">${product.name}</div>
            <div class="product-price">$${product.price.toFixed(2)}</div>
            <div class="product-description">${product.description}</div>
            <div class="product-rating">⭐ ${product.rating} (reviews)</div>
            <button class="btn-add-to-cart" onclick="addToCart(${product.id})">Add to Cart</button>
        `;
        productsGrid.appendChild(productCard);
    });
}

function addToCart(productId) {
    const product = products.find(p => p.id === productId);
    const cartItem = cart.find(item => item.id === productId);

    if (cartItem) {
        cartItem.quantity++;
    } else {
        cart.push({
            ...product,
            quantity: 1
        });
    }

    saveCart();
    updateCartDisplay();
    playSound();
}

function removeFromCart(productId) {
    cart = cart.filter(item => item.id !== productId);
    saveCart();
    updateCartDisplay();
    playSound();
}

function updateQuantity(productId, change) {
    const cartItem = cart.find(item => item.id === productId);
    if (cartItem) {
        cartItem.quantity += change;
        if (cartItem.quantity <= 0) {
            removeFromCart(productId);
        } else {
            saveCart();
            updateCartDisplay();
            playSound();
        }
    }
}

function updateCartDisplay() {
    const cartItemsContainer = document.getElementById('cartItems');
    const cartCount = document.getElementById('cartCount');

    cartCount.textContent = cart.length;

    if (cart.length === 0) {
        cartItemsContainer.innerHTML = `
            <div class="empty-cart">
                <p>Your cart is empty</p>
                <p class="empty-hint">Add items from the products list</p>
            </div>
        `;
    } else {
        cartItemsContainer.innerHTML = '';
        cart.forEach(item => {
            const cartItem = document.createElement('div');
            cartItem.className = 'cart-item';
            cartItem.innerHTML = `
                <div class="item-info">
                    <div class="item-name">${item.name}</div>
                    <div class="item-price">$${item.price.toFixed(2)}</div>
                </div>
                <div class="item-quantity">
                    <button class="qty-btn" onclick="updateQuantity(${item.id}, -1)">-</button>
                    <span class="qty-display">${item.quantity}</span>
                    <button class="qty-btn" onclick="updateQuantity(${item.id}, 1)">+</button>
                </div>
                <div>$${(item.price * item.quantity).toFixed(2)}</div>
                <button class="btn-remove" onclick="removeFromCart(${item.id})">Remove</button>
            `;
            cartItemsContainer.appendChild(cartItem);
        });
    }

    updateCartSummary();
}

function updateCartSummary() {
    const subtotal = cart.reduce((total, item) => total + (item.price * item.quantity), 0);
    const tax = subtotal * 0.1;
    const total = subtotal + tax;

    document.getElementById('subtotal').textContent = '$' + subtotal.toFixed(2);
    document.getElementById('tax').textContent = '$' + tax.toFixed(2);
    document.getElementById('total').textContent = '$' + total.toFixed(2);
}

function clearCart() {
    if (cart.length === 0) {
        alert('Cart is already empty!');
        return;
    }

    if (confirm('Are you sure you want to clear your cart?')) {
        cart = [];
        saveCart();
        updateCartDisplay();
        playSound();
    }
}

function checkout() {
    if (cart.length === 0) {
        alert('Your cart is empty!');
        return;
    }

    const subtotal = cart.reduce((total, item) => total + (item.price * item.quantity), 0);
    const tax = subtotal * 0.1;
    const total = subtotal + tax;

    const orderSummary = cart.map(item => `${item.name} x${item.quantity}`).join(', ');
    const order = {
        date: new Date().toLocaleString(),
        items: orderSummary,
        total: total.toFixed(2),
        itemCount: cart.length
    };

    orderHistory.unshift(order);
    if (orderHistory.length > 15) {
        orderHistory.pop();
    }

    saveOrderHistory();
    
    alert(`Order placed successfully!\nTotal: $${total.toFixed(2)}\n\nThank you for your purchase!`);
    
    cart = [];
    saveCart();
    updateCartDisplay();
    displayOrderHistory();
    playSound();
}

function saveCart() {
    localStorage.setItem('cart', JSON.stringify(cart));
}

function saveOrderHistory() {
    localStorage.setItem('orderHistory', JSON.stringify(orderHistory));
}

function displayOrderHistory() {
    const historyContainer = document.getElementById('orderHistory');
    historyContainer.innerHTML = '';

    if (orderHistory.length === 0) {
        historyContainer.innerHTML = '<div style="color: #888; padding: 10px; text-align: center;">No orders yet</div>';
        return;
    }

    orderHistory.forEach((order, index) => {
        const historyItem = document.createElement('div');
        historyItem.className = 'history-item';
        historyItem.title = `${order.items}\nTotal: $${order.total}`;
        historyItem.innerHTML = `
            <strong>Order ${orderHistory.length - index}:</strong> $${order.total} - ${order.date}
        `;
        historyItem.onclick = () => {
            alert(`Order Details:\nDate: ${order.date}\nItems: ${order.items}\nTotal: $${order.total}`);
        };
        historyContainer.appendChild(historyItem);
    });
}

function clearOrderHistory() {
    if (orderHistory.length === 0) {
        alert('History is already empty!');
        return;
    }

    if (confirm('Clear all order history?')) {
        orderHistory = [];
        saveOrderHistory();
        displayOrderHistory();
        playSound();
    }
}

function toggleTheme() {
    document.body.classList.toggle('light-mode');
    const isDark = document.body.classList.contains('light-mode');
    localStorage.setItem('darkMode', JSON.stringify(!isDark));
}

// Initialize on page load
window.addEventListener('DOMContentLoaded', () => {
    initializeProducts();
    updateCartDisplay();
    displayOrderHistory();
});

// Keyboard shortcuts
document.addEventListener('keydown', function(event) {
    if (event.key === 'Escape') {
        clearCart();
    }
    if (event.ctrlKey && event.key === 'Enter') {
        checkout();
    }
});
