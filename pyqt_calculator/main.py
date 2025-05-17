import sys
from PyQt5 import QtCore, QtGui
from decimal import Decimal
from PyQt5.QtWidgets import QApplication, QWidget, QVBoxLayout, QGridLayout, QPushButton, QLineEdit
from PyQt5.QtGui import QPalette, QColor 

class Calculator(QWidget):
    def __init__(self):
        super().__init__()
        self.setWindowTitle('Calculator')
        self.setLayout(QVBoxLayout())
        self.last_pressed = []
        
        # Create the input field
        self.input_field = QLineEdit()
        self.input_field.setReadOnly(True)
        self.input_field.setAlignment(QtCore.Qt.AlignRight)
        self.input_field.setText('0')
        self.layout().addWidget(self.input_field)
        
        # Create the buttons with new arrangement
        buttons = [
            ['C', '+/-', '%', '÷'],  # Top row with control buttons
            ['7', '8', '9', '×'],
            ['4', '5', '6', '−'],
            ['1', '2', '3', '+'],
            ['0', '00', '.', '=']    # = button is now under plus
        ]
        
        grid_layout = QGridLayout()
        
        for row, button_row in enumerate(buttons):
            for col, button_label in enumerate(button_row):
                button = QPushButton(button_label)
                button.clicked.connect(self.button_clicked)
                button.setObjectName("calculatorButton")
                if button_label in ['÷', '×', '−', '+', '+/-', '=', '%', 'C']:
                    button.setObjectName("mathButton")
                grid_layout.addWidget(button, row, col)
        
        self.layout().addLayout(grid_layout)
        self.current_value = ''
        
        # Set the colors and styles
        self.setStyleSheet('''
    QPushButton#calculatorButton {
        background-color: #333333; /* Тёмно-серый цвет для кнопок */
        color: white;
        font-size: 16px; /* Уменьшенный шрифт */
        padding: 8px; /* Уменьшенные внутренние отступы */
        min-width: 40px; /* Уменьшение ширины кнопок */
        min-height: 40px; /* Уменьшение высоты кнопок */
        border-radius: 20px; /* Закругление кнопок */
    }
    QPushButton#mathButton {
        background-color: #FFA500; /* Жёлтый цвет для кнопок */
        color: white;
        font-size: 16px;
        padding: 8px;
        min-width: 40px;
        min-height: 40px;
        border-radius: 20px;
    }
    QPushButton#controlButton {
        background-color: #DDDDDD; /* Светло-серый цвет для кнопок */
        color: black;
        font-size: 16px;
        padding: 8px;
        min-width: 40px;
        min-height: 40px;
        border-radius: 20px;
    }
    QWidget {
        background-color: #000000; /* Чёрный фон для всего окна */
    }
    QLineEdit {
        background-color: #000000; /* Чёрный цвет поля ввода */
        color: white; /* Белый текст */
        font-size: 18px; /* Уменьшенный шрифт для ввода */
        padding: 8px; /* Уменьшенные отступы внутри поля ввода */
        border-radius: 5px; /* Лёгкое закругление углов */
        margin-top: 10px; /* Отступ сверху */
        margin-bottom: 10px; /* Отступ снизу */
    }
    QPushButton#calculatorButton:hover, QPushButton#mathButton:hover, QPushButton#controlButton:hover {
        background-color: #777777; /* Серый цвет при наведении */
    }
    QPushButton#calculatorButton:pressed, QPushButton#mathButton:pressed, QPushButton#controlButton:pressed {
        background-color: #555555; /* Тёмно-серый цвет при нажатии */
    }
''')

    def symbdup(self, text):
        if text in ['+', '-', '*', '/']:
            if len(self.current_value) > 0 and self.current_value[-1] in ['+', '-', '*', '/']:
                # Replace the last entered symbol with the new symbol
                self.current_value = self.current_value[:-1] + text
            else:
                self.current_value += text
        else:
            self.current_value += text

        self.input_field.setText(self.current_value)

    def button_clicked(self):
        button = self.sender()
        text = button.text()

        if text == '=':
            try:
                result = eval(self.current_value)
                # Limit displayed numbers to 13 digits
                result = round(result, 6)
                result = "{:.13g}".format(Decimal(str(result)))
                self.input_field.setText(result)
                self.current_value = str(result)
                self.last_pressed = '='
            except (SyntaxError, ZeroDivisionError):
                self.input_field.setText('Error')
                self.current_value = ''
        elif text == 'C':
            self.input_field.clear()
            self.current_value = ''
            self.input_field.setText('0')
            self.last_pressed = []
        elif self.current_value == '0':
            if text != '0' and text != '.':
                self.current_value = text
                self.input_field.setText(text)
        elif self.current_value == '00':
            if text != '0':
                self.current_value = text
                self.input_field.setText(text)
        elif self.current_value == '0' or self.current_value == '00':
            self.current_value = text
            self.input_field.setText(text)
        elif self.last_pressed == '=' and text.isnumeric():
            self.input_field.setText(text)
            self.current_value = text
            self.last_pressed = text
        elif text == '÷':
            text = '/'
            self.symbdup(text)
        elif text == '−':
            text = '-'
            self.symbdup(text)
        elif text == '×':
            text = '*'
            self.symbdup(text)
        elif text == '+/-':
            if self.current_value:
                if self.current_value.startswith('-'):
                    self.current_value = self.current_value[1:]
                else:
                    self.current_value = '-' + self.current_value
                self.input_field.setText(self.current_value)
        elif text == '%':
            text = '/100'
            self.symbdup(text)
        else:
            self.symbdup(text)

if __name__ == '__main__':
    app = QApplication(sys.argv)
    calculator = Calculator()
    
    # Set the window background color
    palette = calculator.palette()
    palette.setColor(QPalette.Window, QColor("#D3D3D3"))
    calculator.setPalette(palette)
    
    calculator.show()
    sys.exit(app.exec_())