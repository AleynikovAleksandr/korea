import datetime

def log_action(action):
    """Логирование действий в файл calculator_log.txt"""
    try:
        with open('calculator_log.txt', 'a', encoding='utf-8') as f:
            timestamp = datetime.datetime.now().strftime('%Y-%m-%d %H:%M:%S')
            log_entry = f"[{timestamp}] {action}\n"
            f.write(log_entry)
    except Exception as e:
        print(f"Logging error: {str(e)}")