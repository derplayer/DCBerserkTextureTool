# DC Berserk Texture Tool
**Sword of the Berserk: Guts' Rage - TEX File Packer/Unpacker - Sega Dreamcast**

This tool allows to extract and repack all *.TEX files from the gameSword of the Berserk: Guts' Rage.

How to use?
* Start the tool and select the extracted "TEXTURES" folder from the game image.
* The tool will unpack all files inside the *.TEX files and convert those to *.PVR
* You can freely edit them, just dont move or rename them.
* Also dont change the Compression and Image type of the PVR.
* When you are done just press the "Repack" button in the Tool. All changes from the PVR files will be reapplied to the TEX file.

![Alt text](./pictures/2.jpg "image")

### Video Undub Proof of Concept (JPN Voice, ENG subtitles)
[![Preview](https://img.youtube.com/vi/aAFzFH7KNEU/0.jpg)](https://www.youtube.com/watch?v=aAFzFH7KNEU)

### Video Undub Proof of Concept (ENG Voice, ENG subtitles)
[![Preview](https://img.youtube.com/vi/j8GXRnm-Gsc/0.jpg)](https://www.youtube.com/watch?v=j8GXRnm-Gsc)

## Sword of the Berserk: Guts Rage - Dreamcast *.TEX format specification
```
0x00 - 0x03 - Count of Files in ".TEX" file, lets say 2 as example in 04.TEX ( 02 00 00 00 )
0x04 - 0x07 - Pointer to Texture 1
0x08 - 0x0B - Pointer to Texture 2
	0x0C - 0x1B - 16 byte header of Texture 1 (pvr-like)
		0x00 - 0x03 - global-index-like id (like gbix in pvr)
		0x04 - PVR-like image data format settings byte 1 (Pixel Format)
		0x05 - PVR-like image data format settings byte 2 (Data Format)
		0x06 - 0x09 - Image Width
		0x0a - 0x0d - Image Height
		0x0e - 0x0f - Header end/reserved (0x00)
		0x?? - 0x?? - Image data
	[...] - 16 byte header of Texture 2
```