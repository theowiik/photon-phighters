import subprocess

def run_script(script_name):
    result = subprocess.run(['python3', script_name], capture_output=True, text=True)
    print(f'Running {script_name}')
    print(result.stdout)

def main():
    scripts = ['finder.py', 'converter.py', 'coolizer.py']
    
    for script in scripts:
        run_script(script)

if __name__ == "__main__":
    main()
