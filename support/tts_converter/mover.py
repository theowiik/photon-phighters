# Author: ChatGPT-4 from OpenAI
# Description: Script to find '.movertarget' and copy all .mp3 files from 'output' directory

import os
import shutil
from collections import deque


def bfs(directory):
    # Ensures it doesn't go beyond root directory
    while directory != os.path.dirname(directory):
        queue = deque([directory])

        while queue:
            dir_path = queue.popleft()
            try:
                for filename in os.listdir(dir_path):
                    filepath = os.path.join(dir_path, filename)
                    if os.path.isfile(filepath) and filename == '.movertarget':
                        return dir_path
                    elif os.path.isdir(filepath):
                        queue.append(filepath)
            except PermissionError:
                pass  # Ignoring directories with permission errors

        # If .movertarget not found in the current directory and its subdirectories
        directory = os.path.dirname(directory)  # Go up to the parent directory

    return None  # Returns None if '.movertarget' not found


def copy_files(target_directory):
    source_directory = os.path.join(os.getcwd(), 'output')
    file_list = os.listdir(source_directory)

    for file in file_list:
        if file.endswith('.mp3'):
            src = os.path.join(source_directory, file)
            dst = os.path.join(target_directory, file)
            # Using shutil.copy() to copy the files instead of moving them
            shutil.copy(src, dst)


def main():
    target_directory = bfs(os.getcwd())

    if target_directory:
        print(f"The .movertarget directory is: {target_directory}")
        answer = input("Do you want to copy the .mp3 files? (yes/no): ")

        if answer.lower() == 'yes':
            copy_files(target_directory)
            print("Files have been copied successfully.")
        else:
            print("File copying cancelled by the user.")
    else:
        print("No .movertarget file found.")


if __name__ == "__main__":
    main()
