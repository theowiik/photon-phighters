import os
from pathlib import Path

# Should be ran from the same directory as this file

"""
############################################################
#                                                          #
#  ####  ###### #####   #####  ###### #####   ##   #####   #
# #        ##   #    #  #    # #      #    # #  #  #    #  #
#  ####    ##   #    #  #    # #####  #    #    #  #    #  #
#      #   ##   #####   #####  #      #####     #  #    #  #
# #    #   ##   #       #   #  #      #         #  #    #  #
#  ####   ####  #       #    # ###### #        #  #####    #
#                                                          #
#     Written by ChatGPT, an AI developed by OpenAI        #
############################################################
"""

# Get the current working directory
current_directory = Path.cwd()

# Traverse upwards until we find the 'Scripts' directory
scripts_directory = None
for parent in current_directory.parents:
    potential_directory = parent / 'Scripts'
    if potential_directory.is_dir():
        scripts_directory = potential_directory
        break

# If we couldn't find the Scripts directory, inform the user and exit
if scripts_directory is None:
    print("Could not find 'Scripts' directory")
else:
    # Go to Scripts/PowerUps/Appliers
    appliers_directory = scripts_directory / 'PowerUps' / 'Appliers'

    if not appliers_directory.is_dir():
        print(f"Could not find directory: {appliers_directory}")
    else:
        # Create a list of all .cs files
        cs_files = list(appliers_directory.glob('*.cs'))

        # Prepare to write file names to output
        output_directory = current_directory / 'output'
        output_directory.mkdir(exist_ok=True)
        output_file = output_directory / 'powerups.txt'

        # Open the file in write mode
        with output_file.open('w') as f:
            # Print out the .cs files' names (without .cs extension)
            for cs_file in cs_files:
                # Write the file name (without .cs extension) to the output file
                f.write(f"{cs_file.stem}\n")
