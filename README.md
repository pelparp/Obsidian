# Obsidian
OSTAP Malware String Extractor

## About Obsidian
Obsidian is a tool that attempts to extract strings from the OSTAP malware family. The tool takes in a .jse file that contains the obfuscated code and outputs decoded strings to stdout. 

## About OSTAP
https://malpedia.caad.fkie.fraunhofer.de/details/js.ostap
https://www.intrinsec.com/deobfuscating-hunting-ostap/
https://www.trendmicro.com/en_us/research/19/h/latest-trickbot-campaign-delivered-via-highly-obfuscated-js-file.html

## Usage

Obsidian.exe -i malware.jse -o C:\temp\output

  -i, --input     Required. Absolute path of JSE file to be processed.

  -o, --output    Output path

  --help          Display this help screen.

  --version       Display version information.
