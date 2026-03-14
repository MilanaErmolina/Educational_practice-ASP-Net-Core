$(document).ready(function () {
    let display = $(".display");

    function updateDisplay(value) {
        display.text(value);
    }

    function calculateExpression(expr) {
        $.ajax({
            url: '/api/calculator/calculate',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ expression: expr }),
            success: function (response) {
                updateDisplay(response.result.toString());
            },
            error: function (xhr, status, error) {
                updateDisplay("Error!");
                console.error(error);
            }
        });
    }

    $(".button").on("click", function () {
        let value = $(this).text();
        handleInput(value);
    });

    $(document).on("keyup", function (e) {
        const key = e.key;
        if (key === "Enter") {
            handleInput("=");
        } else if (key === "Escape" || key === "Delete") {
            handleInput("AC");
        } else if (key === "Backspace") {
            let current = display.text();
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

    function handleInput(value) {
        let current = display.text();

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
                let expr = current + "/100";
                calculateExpression(expr);
            }
            return;
        }
        if (value === "=") {
            if (current !== "Error!") {
                calculateExpression(current);
            }
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
});