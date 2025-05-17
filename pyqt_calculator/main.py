import sys
from PyQt5.QtWidgets import QApplication, QMainWindow
from PyQt5.uic import loadUi
from utils.logger import log_action

class Calculator(QMainWindow):
    def __init__(self):
        super().__init__()
        loadUi('calculator.ui', self)
        self.setup_buttons()
        log_action("Application started")

    def setup_buttons(self):
        # Цифры
        self.btn_0.clicked.connect(lambda: self.add_to_display('0'))
        self.btn_1.clicked.connect(lambda: self.add_to_display('1'))
        self.btn_2.clicked.connect(lambda: self.add_to_display('2'))
        self.btn_3.clicked.connect(lambda: self.add_to_display('3'))
        self.btn_4.clicked.connect(lambda: self.add_to_display('4'))
        self.btn_5.clicked.connect(lambda: self.add_to_display('5'))
        self.btn_6.clicked.connect(lambda: self.add_to_display('6'))
        self.btn_7.clicked.connect(lambda: self.add_to_display('7'))
        self.btn_8.clicked.connect(lambda: self.add_to_display('8'))
        self.btn_9.clicked.connect(lambda: self.add_to_display('9'))
        
        # Операции
        self.btn_add.clicked.connect(lambda: self.add_to_display('+'))
        self.btn_subtract.clicked.connect(lambda: self.add_to_display('-'))
        self.btn_multiply.clicked.connect(lambda: self.add_to_display('*'))
        self.btn_divide.clicked.connect(lambda: self.add_to_display('/'))
        self.btn_equals.clicked.connect(self.calculate)
        self.btn_clear.clicked.connect(self.clear_display)
        self.btn_dot.clicked.connect(lambda: self.add_to_display('.'))

    def add_to_display(self, text):
        current = self.display.text()
        self.display.setText(current + text)
        log_action(f"Button pressed: {text}")

    def clear_display(self):
        self.display.setText("")
        log_action("Display cleared")

    def calculate(self):
        try:
            expression = self.display.text()
            result = eval(expression)
            self.display.setText(str(result))
            log_action(f"Calculation: {expression} = {result}")
        except Exception as e:
            self.display.setText("Error")
            log_action(f"Calculation error: {str(e)}")

if __name__ == '__main__':
    app = QApplication(sys.argv)
    window = Calculator()
    window.show()
    sys.exit(app.exec_())