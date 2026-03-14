let display = document.querySelector(".display");
let buttons = Array.from(document.querySelectorAll(".button"));

function updateDisplay(value) {
    display.innerText = value;
}

function calculate() {
    try {
        let result = eval(display.innerText);
        if (typeof result === "number" && !Number.isInteger(result)) {
            result = parseFloat(result.toFixed(10));
        }
        updateDisplay(result.toString());
    } catch (e) {
        updateDisplay("Error!");
    }
}

function handleInput(value) {
    let current = display.innerText;

    if (value === "AC") {
        updateDisplay("0");
        return;
    }
    if (value === "+/-") {
        if (current !== "0" && current !== "Error!") {
            if (current.startsWith("-")) {
                updateDisplay(current.slice(1));
            } else {
                updateDisplay("-" + current);
            }
        }
        return;
    }

    if (value === "%") {
        if (current !== "Error!") {
            try {
                let result = eval(current) / 100;
                updateDisplay(result.toString());
            } catch {
                updateDisplay("Error!");
            }
        }
        return;
    }

    if (value === "=" || value === "Enter") {
        calculate();
        return;
    }

    if (current === "Error!") {
        if (value.match(/[0-9]/)) {
            updateDisplay(value);
        } else {
            updateDisplay("0" + value);
        }
        return;
    }

    if (value === ".") {
        let parts = current.split(/[\+\-\*\/]/);
        let lastPart = parts[parts.length - 1];
        if (!lastPart.includes(".")) {
            updateDisplay(current + ".");
        }
        return;
    }

    if (["+", "-", "*", "/"].includes(value)) {
        let lastChar = current.slice(-1);
        if (["+", "-", "*", "/"].includes(lastChar)) {
            updateDisplay(current.slice(0, -1) + value);
        } else {
            updateDisplay(current + value);
        }
        return;
    }

    if (current === "0") {
        updateDisplay(value);
    } else {
        updateDisplay(current + value);
    }
}

buttons.forEach(button => {
    button.addEventListener("click", e => handleInput(e.target.innerText));
});

document.addEventListener("keyup", e => {
    const key = e.key;

    if (key === "Enter") {
        handleInput("=");
    } else if (key === "Escape" || key === "Delete") {
        handleInput("AC");
    } else if (key === "Backspace") {
        let current = display.innerText;
        if (current === "Error!") {
            updateDisplay("0");
        } else if (current.length > 1) {
            updateDisplay(current.slice(0, -1));
        } else {
            updateDisplay("0");
        }
    } else if (key === "%") {
        handleInput("%");
    } else if (key === ".") {
        handleInput(".");
    } else if (["+", "-", "*", "/"].includes(key)) {
        handleInput(key);
    } else if (key.match(/[0-9]/)) {
        handleInput(key);
    } else if (key === "=") {
        handleInput("=");
    }
});