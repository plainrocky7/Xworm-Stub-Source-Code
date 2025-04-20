# Decompiled XWorm Stub (For Analysis Only)

> ⚠️ **DISCLAIMER**: This is a decompiled stub from the XWorm RAT, provided strictly for educational and research purposes. Do **not** attempt to use this code maliciously. The author of this repository does not condone or support illegal activity.

---

## 💡 What Is This?

Basically, I grabbed an XWorm stub off [tria.ge](https://tria.ge/) and decompiled it using **ILSpy**.  
Not entirely sure if it compiles — it probably does — but I haven't tested it since I'm not on Windows and I'm too lazy to try building it on Linux.

This is mainly for **static analysis** and **educational purposes**, not for execution.

---

## 📄 Breakdown

- **`Settings.cs`** – Contains hardcoded values like:
  - Host / IP
  - Port
  - AES Key  
  These are the main indicators for C2 communication.

- **`Helper.cs`** – General info-gathering:
  - Active window title
  - Client ID
  - Mutex value
  - Probably a few other identifiers

- **`Main.cs`** – Core logic:
  - USB spreading
  - Plugin loading
  - Initial startup routines
  - And other stub-level stuff

- **`XLogger.cs`** – Basic keylogger implementation.  
  Nothing too fancy, but it works.

---

## 📌 Version Info

The stub appears to be from **XWorm v5**, likely in the **5.0–5.6** range.  
Exact version is hard to tell from the decompiled code, but the structure and features line up with samples from that era.

---

## 📚 Want to Learn More?

Here are a couple good resources if you're digging deeper into XWorm:

- [Malpedia - XWorm](https://malpedia.caad.fkie.fraunhofer.de/details/win.xworm)  
  Overview, behavior, version history.

- [ANY.RUN XWorm Analysis](https://any.run/malware-trends/xworm)  
  Live sandbox runs and behavioral data.

---

## ⚠️ Final Notes

This repo is **for analysis only**. Don’t try to compile and run this unless you really know what you're doing and you're in a safe, isolated environment.

Treat this like an unexploded landmine: useful, but dangerous in the wrong hands.

---

