import tensorflow as tf
import numpy as np
from sklearn.preprocessing import LabelEncoder, MinMaxScaler
import json
import os

def log(message):
    log_file_path = "python_log.txt"
    with open(log_file_path, "a", encoding="utf-8") as log_file:
        log_file.write(message + "\n")

current_path = os.path.abspath(__file__)
while not current_path.endswith("Assets"):
    current_path = os.path.dirname(current_path)

unity_assets_path = os.path.join(current_path, "AIModels")
model_path = os.path.join(unity_assets_path, "text_model.keras")
dataset_path = os.path.join(current_path, "Dataset", "Dataset.json")

log(f"Dataset path: {dataset_path}")

if not os.path.exists(dataset_path):
    log(f"[ERROR] Dataset not found at: {dataset_path}")
    raise FileNotFoundError(f"Dataset not found at: {dataset_path}")

with open(dataset_path, "r", encoding="utf-8") as file:
    data = json.load(file)

log(f"[INFO] Dataset loaded. Sample data: {data[:2]}") 

X = np.array(
    [[d["SentimentScore"], d["ManipulativeWordRatio"], d["LexicalDiversity"], d["SubjectivityScore"]] for d in data])
y_labels = [d["Label"] for d in data]
y_conclusions = [d["Conclusion"] for d in data]

log(f"[INFO] Normalizing data...")  
scaler = MinMaxScaler()
X = scaler.fit_transform(X)

label_encoder = LabelEncoder()
y_labels_encoded = label_encoder.fit_transform(y_labels)  

conclusion_encoder = LabelEncoder()
y_conclusions_encoded = conclusion_encoder.fit_transform(y_conclusions)  

if os.path.exists(model_path):
    model = tf.keras.models.load_model(model_path)
    log(f"Model loaded from: {model_path}")
else:
    input_layer = tf.keras.layers.Input(shape=(4,))
    hidden_layer = tf.keras.layers.Dense(8, activation="relu")(input_layer)

    label_output = tf.keras.layers.Dense(len(set(y_labels)), activation="softmax", name="label_output")(hidden_layer)
    conclusion_output = tf.keras.layers.Dense(len(set(y_conclusions)), activation="softmax", name="conclusion_output")(
        hidden_layer)

    model = tf.keras.Model(inputs=input_layer, outputs=[label_output, conclusion_output])
    model.compile(optimizer="adam",
                  loss=["sparse_categorical_crossentropy", "sparse_categorical_crossentropy"],
                  metrics=["accuracy", "accuracy"])

    early_stopping = tf.keras.callbacks.EarlyStopping(
        monitor="loss", patience=20, restore_best_weights=True
    )

    log(f"[INFO] Training model...")
    model.fit(X, [y_labels_encoded, y_conclusions_encoded], epochs=150, batch_size=8, callbacks=[early_stopping])
    model.save(model_path)
    log(f"Model saved to: {model_path}")


def predict_text(json_file_path):
    try:
        log(f"📂 Checking file path: {json_file_path}")

        if not os.path.exists(json_file_path):
            log("❌ File does not exist!")
            return "Error", "File not found"

        if os.path.getsize(json_file_path) == 0:
            log("❌ File is empty!")
            return "Error", "Empty file"

        with open(json_file_path, "r", encoding="utf-8-sig") as file:
            content = file.read()
            log(f"📄 File content:\n{content}")
            data = json.loads(content)

        log(f"[INFO] Input data to model: {data}")

        input_data = np.array([[data[key] for key in
                                ["SentimentScore", "ManipulativeWordRatio", "LexicalDiversity", "SubjectivityScore"]]])
        input_data = scaler.transform(input_data)

        log(f"[INFO] Normalized input data: {input_data}")

        pred_label, pred_conclusion = model.predict(input_data)

        log(f"[INFO] Model raw prediction: Label={pred_label}, Conclusion={pred_conclusion}")

        label_index = np.argmax(pred_label)
        conclusion_index = np.argmax(pred_conclusion)

        decoded_label = label_encoder.inverse_transform([label_index])[0]
        decoded_conclusion = conclusion_encoder.inverse_transform([conclusion_index])[0]

        log(f"[INFO] Prediction: Label={decoded_label}, Conclusion={decoded_conclusion}")
        return str(decoded_label), str(decoded_conclusion)

    except Exception as e:
        log(f"[ERROR] Python Error: {e}")
        return "Python Error", str(e)
