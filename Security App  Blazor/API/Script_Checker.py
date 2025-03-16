from fastapi import FastAPI, HTTPException, UploadFile, File, Request
from pydantic import BaseModel
from typing import List
from bs4 import BeautifulSoup
import requests
import hashlib
import re
import os

app = FastAPI()

def extract_scripts(url):
    try:
        response = requests.get(url)
        if response.status_code == 200:
            soup = BeautifulSoup(response.text, 'html.parser')
            scripts = []
            for script in soup.find_all('script'):
                if script.has_attr('src'):
                    scripts.append({'type': 'external', 'src': script['src']})
                else:
                    scripts.append({'type': 'inline', 'content': script.text})
            return scripts
        else:
            return None
    except Exception as e:
        print(f"An error occurred: {e}")
        return None

def download_script_content(url):
    try:
        response = requests.get(url)
        if response.status_code == 200:
            return response.text
        else:
            print(f"Failed to download script from {url}")
            return None
    except Exception as e:
        print(f"Error downloading script {url}: {e}")
        return None

def hash_script(script_text):
    return hashlib.sha256(script_text.encode('utf-8')).hexdigest()

def append_to_file(file_path, content):
    with open(file_path, 'a', encoding='utf-8') as file:
        file.write(content + "\n")

def clear_file(file_path):
    with open(file_path, 'w', encoding='utf-8') as file:  # 'w' to clear the file
        file.truncate(0)

def compare_or_create_baseline(filename, current_hashes):
    hash_status = {hash: False for hash in current_hashes}
    baseline_hashes = set()
    
    if os.path.exists(filename):
        with open(filename, 'r') as file:
            baseline_hashes = set(file.read().splitlines())
        
        for hash in current_hashes:
            if hash in baseline_hashes:
                hash_status[hash] = True
    else:
        with open(filename, 'a') as file:
            for hash in current_hashes:
                file.write(hash + "\n")
                file.flush()
    
    return hash_status

class URLRequest(BaseModel):
    url: str

@app.post("/check-url/")
async def check_url(request: Request):
    data = await request.json()
    url = data.get('url')

    if not url:
        return {"error": "No URL Provided"}
    
    scripts = extract_scripts(url)

    if scripts is None:
        return {"error": "No Scripts Found"}
    
    all_scripts_info = []
    files_cleared = set()

    subdomain = url.split("//")[-1].split(".")[0]
    filename = f"{subdomain}.txt"
    unauth_filename = f"{subdomain}_unauth_scripts.txt"

    if unauth_filename not in files_cleared:
        clear_file(unauth_filename)
        files_cleared.add(unauth_filename)

    for script in scripts:
        script_content = download_script_content(script['src']) if script['type'] == 'external' else script['content']
        if script_content is None:
            continue

        script_hash = hash_script(script_content)
        hash_status = compare_or_create_baseline(filename, [script_hash])
        allowed = hash_status.get(script_hash, None)

        script_info = {
            "subdomain": subdomain,
            "hash": script_hash,
            "src": script.get('src', None),
            "type": script['type'],
            "allowed": allowed
        }
        all_scripts_info.append(script_info)

        if not allowed:
            append_to_file(unauth_filename, f"Script hash: {script_hash}\nScript content: {script_content}\n")

    return {"scripts": all_scripts_info}




@app.post("/check-url-file/")
async def check_url_file(file: UploadFile = File(...)):
    contents = await file.read()
    urls = contents.decode("utf-8").splitlines()
    
    all_scripts_info = []
    files_cleared = set()  # To keep track of which files have been cleared this run

    for url in urls:
        scripts = extract_scripts(url)
        if scripts is None:
            continue
        
        subdomain = url.split("//")[-1].split(".")[0]
        filename = f"{subdomain}.txt"
        unauth_filename = f"{subdomain}_unauth_scripts.txt"
        
        if unauth_filename not in files_cleared:
            clear_file(unauth_filename)
            files_cleared.add(unauth_filename)
        
        for script in scripts:
            script_content = download_script_content(script['src']) if script['type'] == 'external' else script['content']
            if script_content is None:
                continue

            script_hash = hash_script(script_content)
            hash_status = compare_or_create_baseline(filename, [script_hash])
            allowed = hash_status.get(script_hash, None)
            
            script_info = {
                "subdomain": subdomain,
                "hash": script_hash,
                "src": script.get('src', None),
                "type": script['type'],
                "allowed": allowed
            }
            all_scripts_info.append(script_info)
            
            if not allowed:
                append_to_file(unauth_filename, f"Script hash: {script_hash}\nScript content: {script_content}\n")
    
    return {"scripts": all_scripts_info}

if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="127.0.0.1", port=8000, reload=True)

