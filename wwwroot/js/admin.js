// ajax agam ile işler
function loadPanelContent(section) {
    fetch(`/panel/${section}`)
        .then(response => response.text())
        .then(html => {
            document.getElementById('icerik-alani').innerHTML = html;

        
            if (section === 'admin') {
                attachAdminButtonEvents();
                attachAdminFormSubmit();
            }
            if (section === 'ayarlar') {
                attachAyarButtonEvents();
                attachAyarFormSubmit();
            }
            if (section === 'hizmetkategorileri') {
                attachKategoriButtonEvents();
                attachKategoriFormSubmit();
            }
            if (section === 'hizmetler') {
                attachHizmetButtonEvents();
                attachHizmetFormSubmit();
            }
            if (section === 'icerikler') {
                attachIcerikButtonEvents();
                attachIcerikFormSubmit();
            }
            if (section === 'referanslar') {
                attachReferansButtonEvents();
                attachReferansFormSubmit(); 
            }
        })
        .catch(error => {
            console.error('İçerik yükleme hatası:', error);
        });
}

// ========================== admin ====================================== ok burası glb

// admin  ekleme düzenleme
function editAdmin(id) {
    fetch('/panel/AdminForm?id=' + id)
        .then(res => res.text())
        .then(html => {
            const container = document.getElementById('adminFormContainer');
            if (container) {
                container.innerHTML = html;
                attachAdminFormSubmit(); 
            }
        })
        .catch(err => console.error('Form yüklenirken hata:', err));
}

// admin  submit 
function attachAdminFormSubmit() {
    const form = document.getElementById('adminForm');
    if (form) {
        form.addEventListener('submit', function (e) {
            e.preventDefault();
            const formData = new FormData(form);

            fetch('/panel/AdminForm', {
                method: 'POST',
                body: formData
            })
            .then(res => res.json())
            .then(data => {
                if (data.success) {
                    loadPanelContent('admin');
                } else {
                    alert(data.message || 'Kayıt başarısız.');
                }
            })
            .catch(err => {
                console.error('Form gönderim hatası:', err);
                alert('Bir hata oluştu. Lütfen tekrar deneyin.');
            });
        });
    }
}

// admin  click event 
function attachAdminButtonEvents() {
    const buttons = document.querySelectorAll('[data-edit-admin]');
    buttons.forEach(button => {
        button.removeEventListener('click', handleAdminEdit); 
        button.addEventListener('click', handleAdminEdit);
    });
}

// admin düzenleme 
function handleAdminEdit(e) {
    const id = this.getAttribute('data-edit-admin') || 0;
    editAdmin(id);
}

// sayfa yüklendiginde admin olayları tanımla
document.addEventListener('DOMContentLoaded', function () {
    attachAdminButtonEvents();
    attachAdminFormSubmit();
});

// ==================??????????????????????========= ayarlar ====================================================

// ayar form submit 
function attachAyarFormSubmit() {
    const form = document.getElementById('ayarForm');
    if (form) {
        form.addEventListener('submit', function (e) {
            e.preventDefault();
            const formData = new FormData(form);

            fetch('/panel/AyarForm', {
                method: 'POST',
                body: formData
            })
            .then(res => res.json())
            .then(data => {
                if (data.success) {
                    loadPanelContent('ayarlar');
                }
            });
        });
    }
}

// ayar  event listener 
function attachAyarButtonEvents() {
    const buttons = document.querySelectorAll('[data-edit-ayar]');
    buttons.forEach(btn => {
        btn.removeEventListener('click', handleAyarEdit); 
        btn.addEventListener('click', handleAyarEdit);
    });
}

// ayar düzenleme 
function handleAyarEdit() {
    const id = this.getAttribute('data-edit-ayar') || 0;
    fetch(`/panel/AyarForm?id=${id}`)
        .then(res => res.text())
        .then(html => {
            document.getElementById('ayarFormContainer').innerHTML = html;
            attachAyarFormSubmit();
        });
}

// ======================//==== kategori ======//==================== ok bura ya

// kategori buton
function attachKategoriButtonEvents() {
    document.querySelectorAll('[data-edit-kategori]').forEach(btn => {
        btn.removeEventListener('click', handleKategoriEdit);
        btn.addEventListener('click', handleKategoriEdit);
    });
}

// kategori formunu yükle
function handleKategoriEdit() {
    const id = this.getAttribute('data-edit-kategori') || 0;
    fetch(`/panel/KategoriForm?id=${id}`)
        .then(res => res.text())
        .then(html => {
            document.getElementById('kategoriFormContainer').innerHTML = html;
            attachKategoriFormSubmit();
        });
}

// kategori submit 
function attachKategoriFormSubmit() {
    const form = document.getElementById('kategoriForm');
    if (form) {
        form.addEventListener('submit', function (e) {
            e.preventDefault();
            const formData = new FormData(form);

            fetch('/panel/KategoriForm', {
                method: 'POST',
                body: formData
            })
            .then(res => res.json())
            .then(data => {
                if (data.success) loadPanelContent('hizmetkategorileri');
            });
        });
    }
}

// kategori sil
function deleteKategori(id) {
    if (confirm("Bu kategoriyi silmek istediğinize emin misiniz?")) {
        fetch('/panel/KategoriSil', {
            method: 'POST',
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            body: `id=${id}`
        })
        .then(res => res.json())
        .then(data => {
            if (data.success) loadPanelContent('hizmetkategorileri');
        });
    }
}

// ==================?????????????======== hizmetler =================================================================

// hizmet  butonları
function attachHizmetButtonEvents() {
    document.querySelectorAll('[data-edit-hizmet]').forEach(btn => {
        btn.removeEventListener('click', handleHizmetEdit);
        btn.addEventListener('click', handleHizmetEdit);
    });
}

// hizmet  yükle
function handleHizmetEdit() {
    const id = this.getAttribute('data-edit-hizmet') || 0;
    fetch(`/panel/HizmetForm?id=${id}`)
        .then(res => res.text())
        .then(html => {
            document.getElementById('hizmetFormContainer').innerHTML = html;
            attachHizmetFormSubmit();
        });
}

// hizmet  submit 
function attachHizmetFormSubmit() {
    const form = document.getElementById('hizmetForm');
    if (form) {
        form.addEventListener('submit', function (e) {
            e.preventDefault();
            const formData = new FormData(form);

            fetch('/panel/HizmetForm', {
                method: 'POST',
                body: formData
            })
            .then(res => res.json())
            .then(data => {
                if (data.success) loadPanelContent('hizmetler');
            });
        });
    }
}

// hizmet sil
function deleteHizmet(id) {
    if (confirm("Bu hizmeti silmek istediğinize emin misiniz?")) {
        fetch('/panel/HizmetSil', {
            method: 'POST',
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            body: `id=${id}`
        })
        .then(res => res.json())
        .then(data => {
            if (data.success) loadPanelContent('hizmetler');
        });
    }
}

// ============????????????????????????=============== içerikler ======================================================

// içerik buton
function attachIcerikButtonEvents() {
    document.querySelectorAll('[data-edit-icerik]').forEach(btn => {
        btn.removeEventListener('click', handleIcerikEdit);
        btn.addEventListener('click', handleIcerikEdit);
    });
}

// içerik düzenleme  yükle
function handleIcerikEdit() {
    const id = this.getAttribute('data-edit-icerik');
    fetch('/panel/IcerikForm?id=' + id)
        .then(res => res.text())
        .then(html => {
            document.getElementById('icerikFormContainer').innerHTML = html;
            attachIcerikFormSubmit();
        });
}

// içerik  submit et
function attachIcerikFormSubmit() {
    const form = document.getElementById('icerikForm');
    if (form) {
        form.addEventListener('submit', function (e) {
            e.preventDefault();
            const formData = new FormData(form);

            fetch('/panel/IcerikForm', {
                method: 'POST',
                body: formData
            })
            .then(res => res.json())
            .then(data => {
                if (data.success) loadPanelContent('icerikler');
            });
        });
    }
}

// içerik silme
function deleteIcerik(id) {
    if (confirm("Bu içeriği silmek istediğinize emin misiniz?")) {
        fetch('/panel/IcerikSil', {
            method: 'POST',
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            body: `id=${id}`
        })
        .then(res => res.json())
        .then(data => {
            if (data.success) loadPanelContent('icerikler');
        });
    }
}

// ========================== referanslar ==========================

// referans düzenleme butonları
function attachReferansButtonEvents() {
    document.querySelectorAll('[data-edit-referans]').forEach(btn => {
        btn.removeEventListener('click', handleReferansEdit);
        btn.addEventListener('click', handleReferansEdit);
    });
}

// referans düzenleme formunu yüklee
function handleReferansEdit() {
    const id = this.getAttribute('data-edit-referans');
    fetch('/panel/ReferansForm?id=' + id)
        .then(res => res.text())
        .then(html => {
            document.getElementById('referansFormContainer').innerHTML = html;
            attachReferansFormSubmit();
        });
}

// referans formunu submit et
function attachReferansFormSubmit() {
    const form = document.getElementById('referansForm');
    if (form) {
        form.addEventListener('submit', function (e) {
            e.preventDefault();

            const formData = new FormData(form);

            fetch('/panel/ReferansForm', {
                method: 'POST',
                body: formData
            })
            .then(res => res.json())
            .then(data => {
                if (data.success) {
                    loadPanelContent('referanslar');
                } else {
                    alert(data.message || "Kayıt başarısız.");
                }
            });
        });
    }
}

// referans silme yeri
function deleteReferans(id) {
    if (confirm("Referansı silmek istiyor musunuz?")) {
        fetch('/panel/ReferansSil', {
            method: 'POST',
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            body: `id=${id}`
        })
        .then(res => res.json())
        .then(data => {
            if (data.success) loadPanelContent('referanslar');
        });
    }
}