# AI Text Analysis: Psychological Influence Detection

A research-based software designed to **detect psychological and emotional manipulation** in texts using a combination of classic algorithms and neural networks.  
Built with **Unity (C#)** for the interface and **Python** for AI processing, this system analyzes user input and provides a detailed breakdown of potential influence techniques.


---

## 📌 Table of Contents

- [Project Description](#project-description)
- [Core Features](#core-features)
- [System Architecture](#system-architecture)
- [How It Works](#how-it-works)
- [Algorithms](#algorithms)
- [Neural Network](#neural-network)
- [Screenshots](#screenshots)
- [Technologies Used](#technologies-used)
- [Installation](#installation)
- [License](#license)
- [Author](#author)

---

## 📖 Project Description

This tool was developed as part of a diploma thesis focused on **AI and Information-Psychological Operations (IPO)**.  
Its purpose is to detect elements of influence in digital text using:
- Linguistic analysis
- Sentiment scoring
- AI-based classification

---

## ✨ Core Features

- 🧠 Sentiment analysis (positive / neutral / negative)
- 🧩 Detection of manipulative language and subjective tone
- 📊 Text complexity & lexical diversity analysis
- ⚙️ Integration of Python neural network via PythonNet
- 🎛️ Intuitive Unity UI for easy input/output
- 📈 Exportable results for academic analysis

---

## 🧱 System Architecture

```
Unity (Frontend)
   ↓ PythonNet Bridge
Python Scripts (Analysis + Neural Network)
   ↓
Results returned to Unity UI
```

---

## 🛠️ How It Works

1. User inputs a block of text in the Unity interface.
2. The text is passed to Python through PythonNet.
3. Multiple algorithms analyze different characteristics:
   - Emotional tone
   - Lexical variation
   - Subjectivity & manipulation
4. Neural network performs classification based on computed features.
5. Final results are displayed in Unity.

---

## 📐 Algorithms

### 🟡 1. Sentiment Analysis  
**Goal:** Detect emotional tone (positive, negative, neutral)  
**Tools Used:** TextBlob or custom polarity scoring  
**How it works:**  
- Each word is evaluated for polarity.
- A total score is computed.
- Based on thresholds, a tone is assigned.

---

### 🔴 2. Manipulative Word Frequency  
**Goal:** Find manipulative or emotionally charged words  
**How it works:**  
- Compare words to a database of manipulative terms (e.g., *truth*, *betrayal*, *freedom*, *hidden*).
- Count how many such terms are found.
- Compute a "manipulation level" score.

---

### 🔵 3. Subjectivity & Lexical Diversity  
**Goal:** Determine if the text is objective or opinionated  
**How it works:**  
- Uses subjectivity scoring (e.g., TextBlob or rule-based).
- Measures type-token ratio (TTR) for lexical diversity.
- Flags extremely subjective, repetitive language.

---

## 🤖 Neural Network

- Developed in Python (Keras or PyTorch)
- Trained on a custom dataset of emotionally and manipulative texts
- Input features:
  - Sentiment score
  - Manipulative word frequency
  - Subjectivity level
  - Lexical diversity score
- Output: Binary classification
  - **Psychologically Influenced**
  - **Not Influenced**

---

## 🖼️ Screenshots

### 🔍 Input Interface
![Input](screenshots/input_ui.png)

### 📊 Output Analysis
![Output](screenshots/output_results.png)

### 🔁 Full Workflow
![Flow](screenshots/full_flow.png)

---

## 🧪 Technologies Used

- **Unity** (C#)
- **Python 3.11**
- **PythonNet** (C#↔Python bridge)
- **NLTK**, **TextBlob**, **Sklearn**, **VADER**
- **Custom Neural Network** (Keras/PyTorch)
- Markdown for documentation

---


## 👩‍💻 Author

**[Your Full Name]**  
Bachelor's Degree Student at **[Your University]**  
📫 Contact: [youremail@example.com]

---

## ⚠️ Disclaimer

This project was developed for educational and research purposes only.  
The software is a prototype and should not be used in critical or commercial systems without further validation.


## 📄 License

This project is licensed under the [**Creative Commons Attribution-ShareAlike 4.0 International License**](https://creativecommons.org/licenses/by-sa/4.0/) — see the [LICENSE](LICENSE.md) file for details.

---
