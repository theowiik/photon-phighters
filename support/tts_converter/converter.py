from gtts import gTTS
import re


def text_to_speech(text, file_name):
    # Initialize the gTTS object
    tts = gTTS(text)

    # Save the speech audio into a file
    tts.save(file_name)


def text_to_readable(text):
    # Add space before capital letters to convert "SomethingHere" to "Something Here"
    readable_text = re.sub(r"(\w)([A-Z])", r"\1 \2", text)
    return readable_text


if __name__ == "__main__":
    with open("output/powerups.txt", "r") as file:
        data = file.readlines()

    # Strip newlines and convert text
    data = [text_to_readable(item.strip()) for item in data]

    for item in data:
        file_name = "output/" + item.strip().lower().replace(" ", "_") + ".mp3"
        text_to_speech(item, file_name)
