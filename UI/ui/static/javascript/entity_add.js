document.addEventListener('DOMContentLoaded', function () {
  // Helper: get CSRF cookie (unchanged)
  function getCookie(name) {
    const cookieValue = document.cookie
      .split('; ')
      .find(row => row.startsWith(name + '='));
    return cookieValue ? decodeURIComponent(cookieValue.split('=')[1]) : null;
  }

  function addDeleteButtons() {
    const rows = document.querySelectorAll('tbody tr');
    rows.forEach(row => {
        const idCell = row.querySelector('th');
        const id = idCell.textContent.trim();

        // Only add Delete button for rows with id != -1 and if not already added
        if (id !== '-1' && !row.querySelector('.btn-delete')) {
        const editBtn = row.querySelector('button.btn-primary, button.btn-success');
        if (editBtn) {
            const deleteBtn = document.createElement('button');
            deleteBtn.textContent = 'Delete';
            deleteBtn.className = 'btn btn-sm btn-danger ms-2 btn-delete';
            deleteBtn.onclick = () => deleteRow(row);
            editBtn.parentElement.appendChild(deleteBtn);
        }
        }
    });
    }

    function deleteRow(rowToDelete) {
        const tbody = rowToDelete.parentElement;
        tbody.removeChild(rowToDelete);

        // Re-index all rows except the new row (-1)
        const rows = tbody.querySelectorAll('tr');
        let newId = 1;
        rows.forEach(row => {
            const idCell = row.querySelector('th');
            if (idCell.textContent.trim() !== '-1') {
            idCell.textContent = newId.toString();
            newId++;
            }
        });
    }

  // Helper: get all used attributes excluding the current row's attribute cell (excludeCell)
  function getUsedAttributes(excludeCell) {
    return Array.from(document.querySelectorAll('tbody tr')).map(row => {
      const attrCell = row.querySelector('td:nth-child(2)');
      if (attrCell === excludeCell) return null;

      // If attribute cell is in edit mode (has select), take its selected value
      const select = attrCell.querySelector('select');
      if (select) return select.value.trim();

      // Otherwise, take plain text content
      return attrCell.textContent.trim();
    }).filter(Boolean);
  }

  // Build attribute dropdown excluding used attributes, but including currentAttr
  function buildAttributeDropdown(currentAttr, excludeCell) {
    const unusedAttributes = window.unusedAttributes || [];
    const usedAttributes = getUsedAttributes(excludeCell);
    // Filter unused attributes by excluding used
    let availableAttributes = unusedAttributes.filter(attr => !usedAttributes.includes(attr));

    // Include currentAttr if not empty and not already included
    if (currentAttr && currentAttr !== '' && !availableAttributes.includes(currentAttr)) {
      availableAttributes.unshift(currentAttr);
    }

    const select = document.createElement('select');
    select.className = 'form-select';

    availableAttributes.forEach(attr => {
      const option = document.createElement('option');
      option.value = attr;
      option.textContent = attr;
      if (attr === currentAttr) option.selected = true;
      select.appendChild(option);
    });

    return select;
  }

  // Create Cancel button with appropriate logic
  function createCancelButton(row, button, originalText, id) {
    const cancelBtn = document.createElement('button');
    cancelBtn.textContent = 'Cancel';
    cancelBtn.className = 'btn btn-sm btn-secondary ms-2';

    cancelBtn.onclick = function () {
        const cells = row.querySelectorAll('td');
        const attributeCell = cells[0];
        const descriptionCell = row.querySelector('.description-cell');

        // Restore original text content
        descriptionCell.innerHTML = descriptionCell.getAttribute('data-original-text');
        attributeCell.innerHTML = attributeCell.getAttribute('data-original-text');

        // Restore button text and styling
        button.textContent = originalText;
        button.classList.remove('btn-success');
        button.classList.add('btn-primary');

        // Show delete button again if this is NOT the new row
        if (id !== '-1') {
            const deleteBtn = row.querySelector('.btn-delete');
            if (deleteBtn) deleteBtn.style.display = '';
        }

        // Remove the cancel button
        cancelBtn.remove();
    };

    return cancelBtn;
}

  // Add a new empty row with id -1 at the end of tbody
  function addNewEmptyRow(tbody) {
    const newRow = tbody.lastElementChild.cloneNode(true);
    newRow.querySelector('th').textContent = '-1';

    const attrCell = newRow.querySelector('td:nth-child(2)');
    const descCell = newRow.querySelector('.description-cell');
    const editBtn = newRow.querySelector('button');

    // Clear content and reset
    attrCell.textContent = '';
    descCell.textContent = '';
    editBtn.textContent = 'Add New Detail';
    editBtn.className = 'btn btn-sm btn-primary';
    editBtn.onclick = () => editRow(editBtn);

    // Remove cancel button if any
    const cancelBtn = newRow.querySelector('.btn-secondary');
    if (cancelBtn) cancelBtn.remove();

    tbody.appendChild(newRow);
    addDeleteButtons();
  }

  // Main editRow function
  window.editRow = function (button) {
    const row = button.closest('tr');
    const id = row.querySelector('th').textContent.trim();

    const attrCell = row.querySelector('td:nth-child(2)');
    const descCell = row.querySelector('.description-cell');
    const originalButtonText = button.textContent;
    const deleteBtn = row.querySelector('.btn-delete');

    if (originalButtonText === 'Edit' || originalButtonText === 'Add New Detail') {
        if (deleteBtn) deleteBtn.style.display = 'none';

      // Save original text values to restore on cancel
      attrCell.setAttribute('data-original-text', attrCell.textContent.trim());
      descCell.setAttribute('data-original-text', descCell.textContent.trim());

      // Replace description with input
      const input = document.createElement('input');
      input.type = 'text';
      input.className = 'form-control';
      input.value = descCell.textContent.trim();
      descCell.textContent = '';
      descCell.appendChild(input);

      // Replace attribute with dropdown excluding used attributes
      const currentAttr = attrCell.textContent.trim();
      attrCell.textContent = '';
      attrCell.appendChild(buildAttributeDropdown(currentAttr, attrCell));

      // Update button UI
      button.textContent = 'Save';
      button.classList.remove('btn-primary');
      button.classList.add('btn-success');

      // Add Cancel button
      const cancelBtn = createCancelButton(row, button, originalButtonText, deleteBtn);
      button.parentElement.appendChild(cancelBtn);

    } else if (originalButtonText === 'Save') {
      // On save click
      const input = descCell.querySelector('input');
      const select = attrCell.querySelector('select');

      const newDescription = input?.value.trim();
      const newAttribute = select?.value.trim();

      if (!newDescription || !newAttribute) {
        alert('Attribute and Description must be provided.');
        return;
      }

      // Update cells with new text
      descCell.textContent = newDescription;
      attrCell.textContent = newAttribute;

      // If it was a new row (id = -1), assign new id and add empty new row
      if (id === '-1') {
        const tbody = row.parentElement;
        const newId = tbody.querySelectorAll('tr').length; // use row count as id
        row.querySelector('th').textContent = newId;
        addNewEmptyRow(tbody);
      }

      // Reset button UI
      button.textContent = 'Edit';
      button.classList.remove('btn-success');
      button.classList.add('btn-primary');
      
        if (deleteBtn) deleteBtn.style.display = '';

      // Remove cancel button
      const cancelBtn = button.parentElement.querySelector('.btn-secondary');
      if (cancelBtn) cancelBtn.remove();
    }
  };

  window.saveNewEntity = async function () {
    const rows = document.querySelectorAll('tbody tr');
    const attributes = [];
    const descriptions = [];

    rows.forEach(row => {
      const id = row.querySelector('th').textContent.trim();
      if (id === '-1') return; // Skip new empty row

      const attr = row.querySelector('td:nth-child(2)').textContent.trim();
      const desc = row.querySelector('.description-cell').textContent.trim();

      if (!attr || !desc) return;

    attributes.push(attr);
    descriptions.push(desc);
    });

    const payload = {
        entity: window.entity,
        attributes: attributes,
        descriptions: descriptions
    };

    try {
      const response = await fetch(window.addEntityUrl, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'X-CSRFToken': getCookie('csrftoken')
        },
        body: JSON.stringify({ details: payload })
      });

      if (!response.ok) {
        const errorData = await response.json();
        alert('Error: ' + (errorData.error || 'Unknown error'));
        return;
      }

      const result = await response.json();
      if (result.new_entity_id) {
        window.location.href = `/internal/configuration/${window.entity.toLowerCase()}/${result.new_entity_id}?entity=${window.entity}`;
      } else {
        alert('Success but no new ID returned.');
      }
    } catch (err) {
      alert('Save failed: ' + err.message);
    }
  };

  addDeleteButtons();
});