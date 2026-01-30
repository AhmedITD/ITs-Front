let display = document.getElementById('display');
let operationDisplay = document.getElementById('operation');
let historyContainer = document.getElementById('history');
let currentInput = '';
let operator = null;
let previousValue = null;
let calculationHistory = JSON.parse(localStorage.getItem('calcHistory')) || [];

// Load theme preference
const isDarkMode = JSON.parse(localStorage.getItem('darkMode') !== 'false');
if (!isDarkMode) {
    document.body.classList.add('light-mode');
    document.querySelector('.calculator').classList.add('light-mode');
}

function playSound() {
    // Create a simple beep sound
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
}

function appendNumber(number) {
    if (number === '0' && currentInput === '0') return;
    if (number === '.' && currentInput.includes('.')) return;
    if (currentInput === '' && number === '.') currentInput = '0';
    
    currentInput += number;
    playSound();
    updateDisplay();
}

function appendOperator(op) {
    if (currentInput === '' && previousValue === null) return;
    
    if (operator !== null && currentInput !== '') {
        calculate();
    }
    
    previousValue = currentInput || previousValue;
    currentInput = '';
    operator = op;
    playSound();
    updateDisplay();
}

function appendPercent() {
    if (currentInput === '') return;
    
    let value = parseFloat(currentInput);
    if (previousValue !== null && operator) {
        const prev = parseFloat(previousValue);
        switch (operator) {
            case '+':
            case '-':
                value = (prev * value) / 100;
                break;
            case '*':
            case '/':
                value = value / 100;
                break;
        }
    } else {
        value = value / 100;
    }
    
    currentInput = value.toString();
    playSound();
    updateDisplay();
}

function toggleSign() {
    if (currentInput === '') return;
    
    let value = parseFloat(currentInput);
    value = -value;
    currentInput = value.toString();
    playSound();
    updateDisplay();
}

function appendSquareRoot() {
    if (currentInput === '') return;
    
    let value = parseFloat(currentInput);
    if (value < 0) {
        alert('Cannot calculate square root of negative number!');
        return;
    }
    
    value = Math.sqrt(value);
    currentInput = value.toString();
    playSound();
    updateDisplay();
}

function appendPower() {
    if (currentInput === '') return;
    
    let value = parseFloat(currentInput);
    value = value * value;
    currentInput = value.toString();
    playSound();
    updateDisplay();
}

function calculate() {
    if (operator === null || previousValue === null || currentInput === '') {
        return;
    }
    
    let result;
    const prev = parseFloat(previousValue);
    const current = parseFloat(currentInput);
    
    switch (operator) {
        case '+':
            result = prev + current;
            break;
        case '-':
            result = prev - current;
            break;
        case '*':
            result = prev * current;
            break;
        case '/':
            if (current === 0) {
                alert('Cannot divide by zero!');
                clearDisplay();
                return;
            }
            result = prev / current;
            break;
        default:
            return;
    }
    
    // Round to avoid floating point errors
    result = Math.round(result * 100000000) / 100000000;
    
    const calculation = `${prev} ${operator} ${current} = ${result}`;
    addToHistory(calculation);
    
    currentInput = result.toString();
    operator = null;
    previousValue = null;
    playSound();
    updateDisplay();
}

function clearDisplay() {
    currentInput = '';
    operator = null;
    previousValue = null;
    playSound();
    updateDisplay();
}

function deleteLast() {
    currentInput = currentInput.slice(0, -1);
    playSound();
    updateDisplay();
}

function updateDisplay() {
    display.value = currentInput || '0';
    
    // Update operation display
    if (operator && previousValue) {
        operationDisplay.textContent = `${previousValue} ${operator}`;
    } else {
        operationDisplay.textContent = '';
    }
}

function toggleTheme() {
    const body = document.body;
    const calculator = document.querySelector('.calculator');
    const isDark = body.classList.toggle('light-mode');
    calculator.classList.toggle('light-mode');
    
    // Save preference
    localStorage.setItem('darkMode', JSON.stringify(!isDark));
}

function addToHistory(calculation) {
    calculationHistory.unshift(calculation);
    if (calculationHistory.length > 10) {
        calculationHistory.pop();
    }
    localStorage.setItem('calcHistory', JSON.stringify(calculationHistory));
    displayHistory();
}

function displayHistory() {
    historyContainer.innerHTML = '';
    calculationHistory.forEach((item, index) => {
        const historyItem = document.createElement('div');
        historyItem.className = 'history-item';
        historyItem.textContent = item;
        historyItem.onclick = () => loadFromHistory(item);
        historyItem.title = 'Click to use this result';
        historyContainer.appendChild(historyItem);
    });
}

function loadFromHistory(calculation) {
    const result = calculation.split('=')[1].trim();
    currentInput = result;
    updateDisplay();
}

function clearHistory() {
    if (confirm('Clear calculation history?')) {
        calculationHistory = [];
        localStorage.setItem('calcHistory', JSON.stringify(calculationHistory));
        displayHistory();
        playSound();
    }
}

// Initialize history display
displayHistory();

// Keyboard support
document.addEventListener('keydown', function(event) {
    const key = event.key;
    
    if (key >= '0' && key <= '9') {
        appendNumber(key);
    } else if (key === '.') {
        appendNumber('.');
    } else if (key === '+' || key === '-' || key === '*' || key === '/') {
        event.preventDefault();
        appendOperator(key);
    } else if (key === 'Enter' || key === '=') {
        event.preventDefault();
        calculate();
    } else if (key === 'Backspace') {
        event.preventDefault();
        deleteLast();
    } else if (key === 'Escape') {
        event.preventDefault();
        clearDisplay();
    }
});
