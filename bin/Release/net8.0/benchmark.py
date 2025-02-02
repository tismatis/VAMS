import subprocess
import re
import pyperclip
import statistics
from tqdm import tqdm

def benchmark(exe_path, runs=10):
    timings = {
        "Parsing time": [],
        "Compiling time": [],
        "Generating VM time": [],
        "Internal Timer": [],
        "Function Main executed in": [],
        "Execution time": []
    }
    
    for _ in tqdm(range(runs), desc="Running benchmarks"):
        result = subprocess.run(exe_path, capture_output=True, text=True, shell=True)
        output = result.stdout.strip()
        for key in timings.keys():
            pattern = rf"{re.escape(key)}\s*:?\s*(\d+)ms"
            match = re.search(pattern, output)
            if match:
                timings[key].append(int(match.group(1)))
    
    return timings

exe_path = "lASILLite.exe"
runs = 25
normalRun = input("Do you want update the excel? (y/n): ").strip()
if normalRun.lower() == "n":
    runs = int(input("Enter the number of runs: ").strip())
results = benchmark(exe_path, runs)

wvyw = input("What value you want? (F/I): ").strip()

if normalRun.lower() == "y":
    clipboard_text = []
    if wvyw.lower() == "i":
        clipboard_text.append("\n".join(map(str, results["Internal Timer"])))
    else:
        clipboard_text.append("\n".join(map(str, results["Function Main executed in"])))

    col_val = input("Enter the column: ").strip()
    name_val = input("Enter the Name: ").strip()
    
    data = [
        name_val,
        clipboard_text,
        "",
        f"=MOYENNE({col_val}2:{col_val}26)",
        f"=MEDIANE({col_val}2:{col_val}26)",
        f"=ARRONDI(CENTILE.INCLURE({col_val}28*1%; 0,01); 4)",
        "",
        f"=ARRONDI({col_val}28 * 100 / B28; 2)",
        f"=100 * B32 / {col_val}32",
        f"=100*C32/{col_val}32"
    ]

    cb = input("Do you want to copy the data to clipboard? (y/n): ").strip()
    if cb.lower() == "y":
        pyperclip.copy("\n".join([str(item) for sublist in data for item in (sublist if isinstance(sublist, list) else [sublist])]))
        print("copied to clipboard!")

wdyw = input("Do you want show average table? (y/n): ").strip()
if wdyw.lower() == "y":
    if wvyw.lower() == "f":
        min = min(results["Function Main executed in"])
        max = max(results["Function Main executed in"])
        avr = statistics.mean(results["Function Main executed in"])
        print("Min:", min, "Max:", max, "Average:", avr)
    elif wvyw.lower() == "i":
        min = min(results["Internal Timer"])
        max = max(results["Internal Timer"])
        avr = statistics.mean(results["Internal Timer"])
        print("Min:", min, "Max:", max, "Average:", avr)

    
