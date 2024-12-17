window.speakText = (text) => {
    const msg = new SpeechSynthesisUtterance(text);
    msg.lang = "pt-BR";
    window.speechSynthesis.speak(msg);
};