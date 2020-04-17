# Crossbell Translation Tool GUI v1.0.0
A tool to assist with translating the PC game Ao no Kiseki.

# Thank You!
This tool can only exist due to https://github.com/Ouroboros/EDDecompiler/
And thanks to the original author, FrantzX.

## What you can extract/translate with this tool
+ Items
+ Equipment
+ Fish
+ Food
+ Location Names
+ Quest text in the Detective's Notebook
+ Magic
+ Crafts
+ Books / Newspapers
+ NPC Names
+ Character Names
+ Monster Text - Names / Descriptions / Ability Names

## What you cannot extract/translate with this tool
+ Text in ED_AO.exe - Mostly the game interface
+ Text in images

## PC Translation
The latest version of this tool allows for the extraction/injection of text into the Chinese PC version. The scripting does look to be a little bit different, so I do not recommend trying to inject a PSP/Japanese translation into the PC/Chinese game.

## Translating Text
In each json file created, there are pairs of lines, Text and Translation. Don't touch the value of the Text line. Just enter your translation as the value of the Translation line. If you don't have a translation, just leave the Translation value as "". This will tell the program not to change that line of text.  
You will notice in the scenario files that many lines where the Text value looks to be a filename or already English text. DO NOT CHANGE THESE LINES. You will break the game.
