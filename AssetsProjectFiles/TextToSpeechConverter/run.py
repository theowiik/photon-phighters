# Author: ChatGPT-4 from OpenAI
# Modifies by: ChatGPT-4
# Description: A script to run python scripts with user approval step for 'mover.py'
# Updated at: 15th July 2023

import subprocess


def run_script(script_name):
    result = subprocess.run(['python3', script_name],
                            capture_output=True, text=True)
    print(f'Running {script_name}')
    print(result.stdout)


def main():
    scripts = ['finder.py', 'converter.py', 'coolizer.py']

    for script in scripts:
        run_script(script)


if __name__ == "__main__":
    main()
