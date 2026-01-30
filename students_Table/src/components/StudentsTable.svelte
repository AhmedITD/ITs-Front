<script>
  import { onMount } from 'svelte';

  let students = [];
  let currentPage = 1;
  const itemsPerPage = 6;

  // Form states
  let showForm = false;
  let editingId = null;
  let formName = '';
  let formImage = '';
  let errors = {};

  onMount(() => {
    // Load from localStorage
    const saved = localStorage.getItem('students');
    if (saved) {
      students = JSON.parse(saved);
    } else {
      // Sample data
      students = [
        { id: 1, name: 'Ahmed Ali', image: 'https://i.pravatar.cc/150?img=1' },
        { id: 2, name: 'Fatima Hassan', image: 'https://i.pravatar.cc/150?img=2' },
        { id: 3, name: 'Mohammed Karim', image: 'https://i.pravatar.cc/150?img=3' },
        { id: 4, name: 'Leila Omar', image: 'https://i.pravatar.cc/150?img=4' },
        { id: 5, name: 'Rashid Ibrahim', image: 'https://i.pravatar.cc/150?img=5' },
        { id: 6, name: 'Huda Jamil', image: 'https://i.pravatar.cc/150?img=6' },
        { id: 7, name: 'Kareem Samir', image: 'https://i.pravatar.cc/150?img=7' },
      ];
      saveStudents();
    }
  });

  function saveStudents() {
    localStorage.setItem('students', JSON.stringify(students));
    students = students; // Trigger reactivity
  }

  function validateForm() {
    errors = {};
    if (!formName.trim()) {
      errors.name = 'Name is required';
    }
    if (!formImage.trim()) {
      errors.image = 'Image URL is required';
    }
    return Object.keys(errors).length === 0;
  }

  function openForm(student = null) {
    showForm = true;
    editingId = null;
    formName = '';
    formImage = '';
    errors = {};

    if (student) {
      editingId = student.id;
      formName = student.name;
      formImage = student.image;
    }
  }

  function closeForm() {
    showForm = false;
    editingId = null;
    formName = '';
    formImage = '';
    errors = {};
  }

  function handleSubmit() {
    if (!validateForm()) return;

    if (editingId) {
      // Edit existing
      const index = students.findIndex(s => s.id === editingId);
      if (index !== -1) {
        students[index].name = formName;
        students[index].image = formImage;
      }
    } else {
      // Add new
      const newStudent = {
        id: Date.now(),
        name: formName,
        image: formImage
      };
      students.push(newStudent);
    }

    saveStudents();
    closeForm();
    currentPage = 1; // Reset to first page
  }

  function deleteStudent(id) {
    if (confirm('Are you sure you want to delete this student?')) {
      students = students.filter(s => s.id !== id);
      saveStudents();
    }
  }

  // Pagination
  $: totalPages = Math.ceil(students.length / itemsPerPage);
  $: paginatedStudents = students.slice(
    (currentPage - 1) * itemsPerPage,
    currentPage * itemsPerPage
  );

  function previousPage() {
    if (currentPage > 1) currentPage--;
  }

  function nextPage() {
    if (currentPage < totalPages) currentPage++;
  }
</script>

<div class="container">
  <div class="header">
    <h1> Students Management</h1>
    <button class="btn btn-primary" on:click={() => openForm()}>
       Add Student
    </button>
  </div>

  {#if showForm}
    <div class="modal-overlay" on:click={closeForm}>
      <div class="modal" on:click|stopPropagation>
        <div class="modal-header">
          <h2>{editingId ? 'Edit Student' : 'Add New Student'}</h2>
          <button class="close-btn" on:click={closeForm}>✕</button>
        </div>

        <form on:submit|preventDefault={handleSubmit}>
          <div class="form-group">
            <label for="name">Name</label>
            <input
              id="name"
              type="text"
              bind:value={formName}
              placeholder="Enter student name"
              class:error={errors.name}
            />
            {#if errors.name}
              <span class="error-msg">{errors.name}</span>
            {/if}
          </div>

          <div class="form-group">
            <label for="image">Image URL</label>
            <input
              id="image"
              type="text"
              bind:value={formImage}
              placeholder="Enter image URL"
              class:error={errors.image}
            />
            {#if errors.image}
              <span class="error-msg">{errors.image}</span>
            {/if}
          </div>

          {#if formImage.trim()}
            <div class="image-preview">
              <img src={formImage} alt="Preview" />
              <p>Preview</p>
            </div>
          {/if}

          <div class="form-actions">
            <button type="button" class="btn btn-secondary" on:click={closeForm}>
              Cancel
            </button>
            <button type="submit" class="btn btn-primary">
              {editingId ? 'Update' : 'Add'} Student
            </button>
          </div>
        </form>
      </div>
    </div>
  {/if}

  <div class="table-container">
    {#if students.length === 0}
      <div class="empty-state">
        <p>📚 No students yet. Click "Add Student" to get started!</p>
      </div>
    {:else}
      <table class="students-table">
        <thead>
          <tr>
            <th class="col-image">Photo</th>
            <th class="col-name">Name</th>
            <th class="col-actions">Actions</th>
          </tr>
        </thead>
        <tbody>
          {#each paginatedStudents as student (student.id)}
            <tr class="student-row">
              <td class="col-image">
                <div class="image-cell">
                  <img src={student.image} alt={student.name} />
                </div>
              </td>
              <td class="col-name">{student.name}</td>
              <td class="col-actions">
                <button
                  class="btn btn-sm btn-edit"
                  on:click={() => openForm(student)}
                  title="Edit"
                >
                  ✏️ Edit
                </button>
                <button
                  class="btn btn-sm btn-delete"
                  on:click={() => deleteStudent(student.id)}
                  title="Delete"
                >
                  🗑️ Delete
                </button>
              </td>
            </tr>
          {/each}
        </tbody>
      </table>

      <div class="pagination">
        <button
          class="btn btn-secondary"
          disabled={currentPage === 1}
          on:click={previousPage}
        >
          ← Previous
        </button>
        <div class="page-info">
          Page {currentPage} of {totalPages} ({students.length} total)
        </div>
        <button
          class="btn btn-secondary"
          disabled={currentPage === totalPages}
          on:click={nextPage}
        >
          Next →
        </button>
      </div>
    {/if}
  </div>
</div>

<style>
  .container {
    max-width: 1200px;
    margin: 0 auto;
  }

  .header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 32px;
    padding-bottom: 20px;
    border-bottom: 2px solid #e0e0e0;
  }

  .header h1 {
    font-size: 2.5rem;
    margin: 0;
    color: #1a1a1a;
    font-weight: 700;
  }

  .table-container {
    background: white;
    border: 1px solid #d0d0d0;
    border-radius: 8px;
    padding: 0;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  }

  .empty-state {
    text-align: center;
    padding: 60px 20px;
    color: #666;
    font-size: 1.1rem;
  }

  .students-table {
    width: 100%;
    border-collapse: collapse;
    margin-bottom: 0;
  }

  .students-table thead {
    background: #f5f5f5;
    border-bottom: 2px solid #d0d0d0;
  }

  .students-table th {
    padding: 16px;
    text-align: left;
    font-weight: 700;
    color: #333;
    font-size: 0.95rem;
    text-transform: uppercase;
    letter-spacing: 0.5px;
  }

  .col-image {
    width: 100px;
    text-align: center;
  }

  .col-name {
    flex: 1;
    min-width: 200px;
  }

  .col-actions {
    width: 200px;
    text-align: right;
  }

  .students-table tbody tr {
    border-bottom: 1px solid #e8e8e8;
    transition: background 0.2s ease;
  }

  .students-table tbody tr:hover {
    background: #f9f9f9;
  }

  .students-table tbody tr:last-child {
    border-bottom: none;
  }

  .students-table td {
    padding: 14px 16px;
    vertical-align: middle;
    color: #333;
  }

  .image-cell {
    text-align: center;
  }

  .image-cell img {
    width: 50px;
    height: 50px;
    border-radius: 50%;
    border: 2px solid #4a90e2;
    object-fit: cover;
    display: inline-block;
  }

  .col-name {
    font-weight: 600;
    color: #1a1a1a;
  }

  .student-row .col-actions {
    display: flex;
    gap: 8px;
    justify-content: flex-end;
  }

  .btn {
    padding: 8px 12px;
    border: none;
    border-radius: 4px;
    font-weight: 600;
    cursor: pointer;
    transition: all 0.3s ease;
    font-size: 0.9rem;
  }

  .btn:disabled {
    opacity: 0.5;
    cursor: not-allowed;
  }

  .btn-primary {
    background: #4a90e2;
    color: white;
    border: 1px solid #3a7bc8;
  }

  .btn-primary:hover:not(:disabled) {
    background: #357abd;
  }

  .btn-secondary {
    background: #e8e8e8;
    color: #333;
    border: 1px solid #d0d0d0;
  }

  .btn-secondary:hover:not(:disabled) {
    background: #d8d8d8;
  }

  .btn-sm {
    padding: 6px 10px;
    font-size: 0.85rem;
  }

  .btn-edit {
    background: #e8f4f8;
    color: #0066b3;
    border: 1px solid #b3d9e8;
  }

  .btn-edit:hover:not(:disabled) {
    background: #d0e8f0;
  }

  .btn-delete {
    background: #fde8e8;
    color: #c0392b;
    border: 1px solid #e8b3b3;
  }

  .btn-delete:hover:not(:disabled) {
    background: #f5d0d0;
  }

  .modal-overlay {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background: rgba(0, 0, 0, 0.5);
    display: flex;
    align-items: center;
    justify-content: center;
    z-index: 100;
  }

  .modal {
    background: white;
    border: 1px solid #d0d0d0;
    border-radius: 8px;
    padding: 24px;
    max-width: 400px;
    width: 90%;
    max-height: 90vh;
    overflow-y: auto;
    box-shadow: 0 4px 16px rgba(0, 0, 0, 0.2);
  }

  .modal-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 20px;
    padding-bottom: 16px;
    border-bottom: 1px solid #e8e8e8;
  }

  .modal-header h2 {
    margin: 0;
    font-size: 1.5rem;
    color: #1a1a1a;
  }

  .close-btn {
    background: none;
    border: none;
    color: #666;
    font-size: 1.5rem;
    cursor: pointer;
    padding: 0;
  }

  .close-btn:hover {
    color: #333;
  }

  .form-group {
    margin-bottom: 20px;
  }

  .form-group label {
    display: block;
    margin-bottom: 8px;
    font-weight: 600;
    color: #333;
  }

  .form-group input {
    width: 100%;
    padding: 10px 12px;
    background: #f9f9f9;
    border: 1px solid #d0d0d0;
    border-radius: 4px;
    color: #333;
    font-size: 1rem;
    transition: all 0.3s ease;
  }

  .form-group input:focus {
    outline: none;
    border-color: #4a90e2;
    background: white;
    box-shadow: 0 0 0 3px rgba(74, 144, 226, 0.1);
  }

  .form-group input.error {
    border-color: #c0392b;
  }

  .error-msg {
    color: #c0392b;
    font-size: 0.85rem;
    display: block;
    margin-top: 4px;
  }

  .image-preview {
    text-align: center;
    margin: 20px 0;
    padding: 16px;
    background: #f5f5f5;
    border-radius: 4px;
    border: 1px solid #e0e0e0;
  }

  .image-preview img {
    width: 80px;
    height: 80px;
    border-radius: 50%;
    border: 2px solid #4a90e2;
    object-fit: cover;
    margin-bottom: 8px;
  }

  .image-preview p {
    margin: 0;
    color: #666;
    font-size: 0.85rem;
  }

  .form-actions {
    display: flex;
    gap: 12px;
    margin-top: 24px;
  }

  .form-actions button {
    flex: 1;
  }

  .pagination {
    display: flex;
    justify-content: center;
    align-items: center;
    gap: 16px;
    padding: 20px 24px;
    border-top: 1px solid #e8e8e8;
  }

  .page-info {
    color: #666;
    font-size: 0.95rem;
    min-width: 200px;
    text-align: center;
  }

  @media (max-width: 768px) {
    .header {
      flex-direction: column;
      gap: 16px;
      align-items: flex-start;
    }

    .header h1 {
      font-size: 1.8rem;
    }

    .students-table {
      font-size: 0.9rem;
    }

    .students-table th,
    .students-table td {
      padding: 10px 8px;
    }

    .students-table th {
      font-size: 0.8rem;
    }

    .image-cell img {
      width: 40px;
      height: 40px;
    }

    .col-actions {
      width: auto;
    }

    .btn-sm {
      padding: 4px 8px;
      font-size: 0.75rem;
    }

    .pagination {
      flex-direction: column;
      gap: 12px;
    }

    .pagination button {
      width: 100%;
    }
  }
</style>
