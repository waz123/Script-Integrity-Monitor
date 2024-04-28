import hashlib
from bs4 import BeautifulSoup 
import re
import os

def extract_scripts():
    file_to_read = input("Please enter the file name: ")
    file_text = read_file(file_to_read)
    
    if file_text is None:
        return []
    try:
        soup = BeautifulSoup(file_text, 'html.parser')
        scripts = [script['src'] if script.has_attr('src') else script.text for script in soup.find_all('script')]
        return scripts
    except Exception as e:
        print(f"An error occurred while parsing the HTML: {e}")
        return []

def read_file(file_path):
    try:
        with open(file_path, 'r', encoding='utf-8') as file:
            return file.read()
    except Exception as e:
        print(f"An error occurred while reading the file: {e}")
        return None

def normalize_script(script_text):
    normalized_text = re.sub(r'https?://[^\s]+', 'https://cart.aimsparking.com', script_text)
    return normalized_text

def hash_script(script_text):
    normalized_text = normalize_script(script_text)
    return hashlib.sha256(normalized_text.encode('utf-8')).hexdigest()


def main():
    scripts = extract_scripts()
    if scripts:  # Check if the list is not empty
        for script in scripts:
            print(hash_script(script))
    else:
        print("No scripts found.")

if __name__ == "__main__":
    main()