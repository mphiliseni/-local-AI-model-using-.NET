async function sendMessage() {

    const input =
        document.getElementById("messageBox");

    const message = input.value;

    if (!message)
        return;

    document
        .getElementById("emptyState")
        ?.remove();

    addMessage(message, "user");

    input.value = "";

    // create assistant bubble and show typing inside it
    const assistantBubble = addMessage("", "assistant");
    showTyping(assistantBubble);

    const res = await fetch("/Api/Chat/stream", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ message: message })
    });

    if (!res.ok || !res.body) {
        hideTyping(assistantBubble);
        assistantBubble.innerText = "Error: unable to contact AI";
        return;
    }

    const reader = res.body.getReader();
    const decoder = new TextDecoder();
    let buffer = "";

    while (true) {
        const { value, done } = await reader.read();
        if (done) break;
        buffer += decoder.decode(value, { stream: true });

        // process SSE events separated by double newline
        const parts = buffer.split('\n\n');
        buffer = parts.pop();
        for (const part of parts) {
            if (!part) continue;
            const lines = part.split('\n').map(l => l.trim());
            for (const line of lines) {
                if (line.startsWith('data:')) {
                    const raw = line.slice(5).trim();
                    try {
                        const text = JSON.parse(raw);
                        // remove typing dots on first chunk
                        if (assistantBubble.classList.contains('typing-dots')) {
                            assistantBubble.classList.remove('typing-dots');
                            assistantBubble.innerText = '';
                        }
                        assistantBubble.innerText += text;
                    } catch (e) {
                        if (assistantBubble.classList.contains('typing-dots')) {
                            assistantBubble.classList.remove('typing-dots');
                            assistantBubble.innerText = '';
                        }
                        assistantBubble.innerText += raw;
                    }
                }
                else if (line.startsWith('event:') && line.includes('done')) {
                    // stream finished signal
                }
            }
        }
    }

    hideTyping(assistantBubble);
}

function addMessage(text, role) {

    const chat = document.getElementById("chatWindow");

    const wrapper = document.createElement('div');
    wrapper.className = role;

    const meta = document.createElement('div');
    meta.className = 'meta';
    const name = document.createElement('strong');
    name.innerText = role === 'user' ? 'You' : 'AI Assistant';
    const time = document.createElement('span');
    time.className = 'time';
    time.innerText = new Date().toLocaleTimeString();
    meta.appendChild(name);
    meta.appendChild(document.createTextNode(' \u00A0 '));
    meta.appendChild(time);

    const bubble = document.createElement('div');
    bubble.className = role === 'user' ? 'bubble-user' : 'bubble-ai';
    bubble.innerText = text;

    wrapper.appendChild(meta);
    wrapper.appendChild(bubble);
    chat.appendChild(wrapper);
    chat.scrollTop = chat.scrollHeight;
    return bubble;
}

function showTyping(bubble) {
    if (bubble) {
        if (!bubble.classList.contains('typing-dots')) {
            bubble.classList.add('typing-dots');
            bubble.innerHTML = '<span></span><span></span><span></span>';
        }
        return;
    }
    const el = document.getElementById('typingIndicator');
    if (el) {
        el.style.display = 'block';
        el.dataset.prior = el.innerHTML;
        el.innerText = 'AI Assistant is typing...';
    }
}

function hideTyping(bubble) {
    if (bubble) {
        if (bubble.classList.contains('typing-dots')) {
            bubble.classList.remove('typing-dots');
            // streaming will set innerText when chunks arrive
        }
        return;
    }
    const el = document.getElementById('typingIndicator');
    if (el) {
        // restore previous innerHTML (dots) if we saved it
        if (el.dataset.prior) {
            el.innerHTML = el.dataset.prior;
            delete el.dataset.prior;
        }
        el.style.display = 'none';
    }
}

function sendPrompt(prompt) {

    document.getElementById(
        "messageBox").value = prompt;

    sendMessage();
}