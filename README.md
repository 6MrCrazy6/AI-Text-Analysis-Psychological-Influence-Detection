<p align="left">
  <img src="Img/Logo.png" alt="Logo" width="250"/>
</p>


# AI Text Analysis: Psychological Influence Detection

A research-based software designed to **detect psychological and emotional manipulation** in texts using a combination of classic algorithms and neural networks.  
Built with **Unity (C#)** for the interface and **Python** for AI processing, this system analyzes user input and provides a detailed breakdown of potential influence techniques.


---

## 📌 Table of Contents

- [Project Description](#-project-description)
- [Core Features](#-core-features)
- [System Architecture](#-system-architecture)
- [How It Works](#-how-it-works)
- [Algorithms](#-algorithms)
- [Neural Network Model](#-neural-network-model)
- [Screenshots](#-screenshots)
- [Technologies Used](#-technologies-used)
- [Author](#-author)
- [Disclaimer](#-disclaimer)
- [License](#-license)

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
   ↓ C# Algorithms
   ↓ PythonNet Bridge
Python Scripts (Neural Network)
   ↓
Results returned to Unity UI
```

---

## 🛠️ How It Works

1. User inputs a block of text in the Unity interface.
2. The text is passed to Python through PythonNet.
3. Multiple algorithms analyze different characteristics:
   - Sentiment Analysis
   - Lexical Diversity (TTR)
   - Subjectivity & manipulation
4. Neural network performs classification based on computed features.
5. Final results are displayed in Unity.

---

## 📐 Algorithms

### 🗣️ Sentiment Analysis

The system uses two established Python libraries:

- **VADER** – A lexicon and rule-based sentiment analysis tool tuned for social media text.
- **TextBlob** – A simple library for processing textual data, providing sentiment polarity.

To compute a unified sentiment score:

<h3 align="center">$S = \frac{V + T}{2}$</h3>

Where:
- `V` = sentiment score from **VADER**
- `T` = sentiment score from **TextBlob**

---

### 🧠 Subjectivity Index

This score measures how opinionated the text is. The formula used:

<h3 align="center">$S_r = \frac{S_u}{T}$</h3>

Where:
- $`S_u`$ = number of subjective sentences
- `T` = total number of sentences

---

### 📚 Lexical Diversity (TTR)

The **Type-Token Ratio (TTR)** is used to evaluate the vocabulary variety:

<h3 align="center">$TTR = \frac{U}{T}$</h3>

Where:
- `U` = number of unique words
- `T` = total number of words

---

### ⚠️ Manipulative Language Detection

The system uses a custom-crafted JSON database containing over **2,000 labeled manipulative phrases**, grouped into categories:

- Generalization  
- Call to action  
- Emotional triggers  
- Fear-based messages  
- Manipulative contrasts  
- False analogies  
- Concept substitution  
- Appeal to patriotism  
- Doubt and uncertainty  
- Urgency and alarmism  

For each detected word/phrase:

<h3 align="center">$M_f = \frac{M}{T}$</h3>

Where:
- `M` = number of manipulative words found in the text
- `T` = total number of words

---

## 🧠 Neural Network Model

The neural network receives the following **four features** as input:

1. Overall sentiment score: `S`
2. Manipulative frequency: $`M_f`$
3. Subjectivity index: $`S_r`$
4. Lexical diversity: `TTR`

### 🔧 Architecture

<p align="center">
  <img src="Img/NeuralNetworkStructure.png" alt="Neural Network Architecture" width="600"/>
</p>

#### Layer Details:
- **Input Layer**: 4 neurons (1 per feature)  
- **Hidden Layer**: 8 neurons, `ReLU` activation  
- **Output Layer**: 2 neurons, `Softmax` activation  
    - Label: classification (e.g. Propaganda / Neutral)  
    - Conclusion: summary of the result

#### Training:
- **Optimizer**: Adam  
- **Loss Function**: `sparse_categorical_crossentropy`  
- **Training Data**: Labeled dataset with annotated propaganda indicators

---

## 🖼️ Screenshots

### 🏠 Main Menu  
![Main Menu](Img/MainMenu.png)  

The main screen of the application consists of the following components:

- **Application Title**
- **Start Analysis** button
- **About the App** button
- **Exit** button

---

### 🔍 Input Interface  
![Input](Img/InputText.png)  

After clicking the **Start Analysis** button, the main window opens with these blocks:

- **Text Input Field** – for entering or pasting content  
- **Word Counter** – tracks the number of words  
- **Analyze** button – starts the text analysis  
- **Clear** button – clears the text field  

---

### 📊 Output Analysis  
![Output](Img/Results.png) 

The screenshot shows the output after analyzing a text fragment from the movie *"Angels & Demons"*.  
The neural network evaluated the content as **neutral** and generated the following conclusion:

> *"The text avoids extreme opinions or manipulative elements and provides straightforward information."*

---

## 🧪 Technologies Used

- **Unity** (C#)
- **Python 3.11**
- **PythonNet** (C#↔Python bridge)
- **NLTK**, **TextBlob**, **Sklearn**, **VADER**
- **Custom Neural Network** (Keras/Tensorflow)
- Markdown for documentation

---


## 👩‍💻 Author

**Inna Varchenko**  
Bachelor's Degree Student at **Central Ukrainian National Technical University]**  
📫 Contact: devilyumeko42@gmail.com

---

## ⚠️ Disclaimer

This project was developed for educational and research purposes only.  
The software is a prototype and should not be used in critical or commercial systems without further validation.

---


## 📄 License

This project is licensed under the [**Creative Commons Attribution-ShareAlike 4.0 International License**](https://creativecommons.org/licenses/by-sa/4.0/) — see the [LICENSE](LICENSE.md) file for details.

---
