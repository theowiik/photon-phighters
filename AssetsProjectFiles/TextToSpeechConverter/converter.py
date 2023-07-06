from gtts import gTTS

def text_to_speech(text, file_name):
    # Initialize the gTTS object
    tts = gTTS(text)

    # Save the speech audio into a file
    tts.save(file_name)

if __name__ == "__main__":
    data = [
        "photon blaster", 
        "health boost"
    ]

    for item in data:
        file_name = "output/" + item.strip().lower().replace(" ", "_") + ".mp3"
        text_to_speech(item, file_name)
