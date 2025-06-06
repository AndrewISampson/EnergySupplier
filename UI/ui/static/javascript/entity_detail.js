document.addEventListener('DOMContentLoaded', function () {
    window.editRow = async function (button) {
        const row = button.closest('tr');
        const id = row.querySelector('th').textContent.trim();
        const cells = row.querySelectorAll('td');
        const attributeCell = cells[0];
        const descriptionCell = row.querySelector('.description-cell');
        const originalButtonTextContent = button.textContent;

        if (originalButtonTextContent === 'Edit'
            || originalButtonTextContent === 'Add New Detail') {
            // Save original values
            const originalDescription = descriptionCell.textContent.trim();
            descriptionCell.setAttribute('data-original-text', originalDescription);

            const input = document.createElement('input');
            input.type = 'text';
            input.className = 'form-control';
            input.value = originalDescription;
            descriptionCell.innerHTML = '';
            descriptionCell.appendChild(input);

            // Attribute dropdown if ID is -1
            if (id === '-1') {
                const unusedAttributes = window.unusedAttributes || [];
                const select = document.createElement('select');
                select.className = 'form-select';

                unusedAttributes.forEach(attr => {
                    const option = document.createElement('option');
                    option.value = attr;
                    option.textContent = attr;
                    select.appendChild(option);
                });

                attributeCell.setAttribute('data-original-text', attributeCell.textContent.trim());
                attributeCell.innerHTML = '';
                attributeCell.appendChild(select);
            }

            button.textContent = 'Save';
            button.classList.remove('btn-primary');
            button.classList.add('btn-success');

            const cancelBtn = document.createElement('button');
            cancelBtn.textContent = 'Cancel';
            cancelBtn.className = 'btn btn-sm btn-secondary ms-2';
            cancelBtn.onclick = function () {
                descriptionCell.innerHTML = descriptionCell.getAttribute('data-original-text');
                if (id === '-1') {
                    attributeCell.innerHTML = attributeCell.getAttribute('data-original-text');
                }
                button.textContent = originalButtonTextContent;
                button.classList.remove('btn-success');
                button.classList.add('btn-primary');
                cancelBtn.remove();
            };
            button.parentElement.appendChild(cancelBtn);

        } else {
            const newDescription = descriptionCell.querySelector('input').value;
            const newAttribute = id === '-1'
                ? attributeCell.querySelector('select').value
                : attributeCell.textContent.trim();

            const response = await fetch(window.updateEntityDetailUrl, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'X-CSRFToken': getCookie('csrftoken')
                },
                body: JSON.stringify({
                    entity: window.entity,
                    entity_id: window.entityId,
                    id: id,
                    new_description: newDescription,
                    new_attribute: newAttribute
                })
            });

            if (response.ok) {
                location.reload();
            } else {
                const errorData = await response.json();
                alert('Update failed: ' + (errorData.error || 'Unknown error'));
            }
        }
    };

    window.getCookie = function (name) {
        const cookieValue = document.cookie
            .split('; ')
            .find(row => row.startsWith(name + '='));
        return cookieValue ? decodeURIComponent(cookieValue.split('=')[1]) : null;
    };
});