import os
import re

# 🔧 Настройки замены
SOURCE = "gateway"
DEST = "auth"

# 🛑 Получаем путь к текущему скрипту
SCRIPT_PATH = os.path.realpath(__file__)

# Сопоставление регистра
def match_case(word, template):
    result = ''
    for w, t in zip(word, template):
        result += w.upper() if t.isupper() else w.lower()
    result += word[len(template):]
    return result

# Замена с учётом регистра
def replace_word(text):
    def repl(m):
        return match_case(DEST, m.group())
    return re.sub(SOURCE, repl, text, flags=re.IGNORECASE)

# Переименование директорий (глубже — сначала вложенные)
for root, dirs, files in os.walk('.', topdown=False):
    for dirname in dirs:
        if re.search(SOURCE, dirname, re.IGNORECASE):
            old_path = os.path.join(root, dirname)
            new_name = replace_word(dirname)
            new_path = os.path.join(root, new_name)
            os.rename(old_path, new_path)

# Замена содержимого и переименование файлов
for root, dirs, files in os.walk('.', topdown=False):
    for filename in files:
        filepath = os.path.join(root, filename)

        # 🛑 Пропускаем сам скрипт
        if os.path.realpath(filepath) == SCRIPT_PATH:
            continue

        # Заменяем содержимое файла
        try:
            with open(filepath, 'r', encoding='utf-8') as f:
                content = f.read()
            new_content = replace_word(content)
            if content != new_content:
                with open(filepath, 'w', encoding='utf-8') as f:
                    f.write(new_content)
        except Exception:
            continue  # Пропускаем бинарные и ошибки

        # Переименование файла
        if re.search(SOURCE, filename, re.IGNORECASE):
            new_filename = replace_word(filename)
            new_filepath = os.path.join(root, new_filename)
            os.rename(filepath, new_filepath)

