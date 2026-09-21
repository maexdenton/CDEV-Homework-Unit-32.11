document.addEventListener('DOMContentLoaded', () => {
    const feedbackForm = document.getElementById('feedbackForm');
    if (!feedbackForm) return;

    feedbackForm.addEventListener('submit', async (e) => {
        e.preventDefault();

        const submitBtn = document.getElementById('submitBtn');
        const fromUserInput = document.getElementById('fromUser');
        const textInput = document.getElementById('feedbackText');

        const fromUser = fromUserInput.value.trim();
        const text = textInput.value.trim();

        if (!fromUser || !text) return;

        submitBtn.disabled = true;

        try {
            const response = await fetch('/Feedback/Add', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    fromUser: fromUser,
                    text: text
                })
            });

            const result = await response.json();

            if (result.success) {
                // Добавляем отзыв в DOM
                appendFeedbackToDom(result.feedback);

                // Очищаем форму
                fromUserInput.value = '';
                textInput.value = '';
            } else {
                alert('Ошибка: ' + result.message);
            }
        } catch (error) {
            console.error('Ошибка при отправке AJAX:', error);
            alert('Не удалось отправить отзыв.');
        } finally {
            submitBtn.disabled = false;
        }
    });
});

// Функция обновления DOM
function appendFeedbackToDom(feedback) {
    const feedbackList = document.getElementById('feedbackList');
    if (!feedbackList) return;

    const card = document.createElement('div');
    card.className = 'card mb-3 feedback-card shadow-sm';
    card.innerHTML = `
        <div class="card-body">
            <div class="d-flex justify-content-between align-items-center mb-2">
                <h6 class="card-subtitle text-primary fw-bold">${escapeHtml(feedback.fromUser)}</h6>
                <small class="text-muted">${feedback.createdAt}</small>
            </div>
            <p class="card-text mb-0">${escapeHtml(feedback.text)}</p>
        </div>
    `;

    feedbackList.insertBefore(card, feedbackList.firstChild);
}

// Экранирование HTML от XSS
function escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}