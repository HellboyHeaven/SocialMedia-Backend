import os
import re

# Сопоставление регистра
def match_case(word, template):
    result = ''
    for w, t in zip(word, template):
        result += w.upper() if t.isupper() else w.lower()
    result += word[len(template):]
    return result

# Замена с учётом регистра
def replace_comment(text):
    def repl(m):
        return match_case('comment', m.group())
    return re.sub(r'comment', repl, text, flags=re.IGNORECASE)

# Переименование директорий (обратно, чтобы сначала глубже)
for root, dirs, files in os.walk('.', topdown=False):
    # Переименовываем директории
    for dirname in dirs:
        if re.search(r'comment', dirname, re.IGNORECASE):
            old_path = os.path.join(root, dirname)
            new_name = replace_comment(dirname)
            new_path = os.path.join(root, new_name)
            os.rename(old_path, new_path)

# Снова обходим — для файлов (пути уже обновлены)
for root, dirs, files in os.walk('.', topdown=False):
    for filename in files:
        filepath = os.path.join(root, filename)

        # Заменяем содержимое файла
        try:
            with open(filepath, 'r', encoding='utf-8') as f:
                content = f.read()
            new_content = replace_comment(content)
            if content != new_content:
                with open(filepath, 'w', encoding='utf-8') as f:
                    f.write(new_content)
        except Exception:
            continue  # Пропустить бинарные/невозможно читаемые

        # Переименование файла
        if re.search(r'comment', filename, re.IGNORECASE):
            new_filename = replace_comment(filename)
            new_filepath = os.path.join(root, new_filename)
            os.rename(filepath, new_filepath)

