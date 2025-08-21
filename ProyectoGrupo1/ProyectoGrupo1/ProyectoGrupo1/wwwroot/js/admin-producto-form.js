(function () {
    const L = window.__LOOKUPS__ || { Categorias: [], Marcas: [], Proveedores: [], Tallas: [], Colores: [] };
    const SEED = window.__SEED__ || { imagenes: [], ptcs: [] };

    const $ = (s, c = document) => c.querySelector(s);
    const $$ = (s, c = document) => Array.from(c.querySelectorAll(s));

    const form = $('#frmProducto');
    const imgsBox = $('#imgs');
    const ptcsBox = $('#ptcs');
    const hiddenBag = $('#hiddenBag');

    function escapeHtml(s) { return String(s ?? '').replace(/[&<>"']/g, m => ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[m])); }
    function fillSelect(select, items, selected) {
        if (!select) return;
        select.innerHTML = ['<option value="">-- Seleccione --</option>']
            .concat(items.map(x => `<option value="${x.Id}">${escapeHtml(x.Nombre)}</option>`))
            .join('');
        const sel = select.dataset.selected ?? selected ?? '';
        if (sel !== '') select.value = String(sel);
    }

    fillSelect($('#CategoriaID'), L.Categorias);
    fillSelect($('#MarcaID'), L.Marcas);
    fillSelect($('#ProveedorID'), L.Proveedores);

    function addImgRow(val = '') {
        const row = document.createElement('div');
        row.className = 'input-group mb-2';
        row.setAttribute('data-img-row', '');
        row.innerHTML = `
      <input type="text" class="form-control" placeholder="https://..." value="${val || ''}">
      <button type="button" class="btn btn-outline-danger" data-remove>Quitar</button>`;
        imgsBox.appendChild(row);
    }

    imgsBox.replaceChildren();
    const imgsSeed = Array.isArray(SEED.imagenes) ? SEED.imagenes : [];
    (imgsSeed.length ? imgsSeed : ['']).forEach(addImgRow);

    function addPtcRow(ptc) {
        const row = document.createElement('div');
        row.className = 'row g-2 align-items-center mb-2';
        row.setAttribute('data-ptc-row', '');
        row.innerHTML = `
      <div class="col-md-4"><select class="form-select" name="talla"></select></div>
      <div class="col-md-4"><select class="form-select" name="color"></select></div>
      <div class="col-md-3"><input type="number" class="form-control" name="stock" value="${ptc?.Stock ?? 0}" min="0"></div>
      <div class="col-md-1 text-end"><button type="button" class="btn btn-outline-danger" data-remove>X</button></div>`;
        fillSelect(row.querySelector('select[name="talla"]'), L.Tallas, ptc?.TallaID);
        fillSelect(row.querySelector('select[name="color"]'), L.Colores, ptc?.ColorID);
        ptcsBox.appendChild(row);
    }

    ptcsBox.replaceChildren();
    const ptcSeed = Array.isArray(SEED.ptcs) ? SEED.ptcs : [];
    (ptcSeed.length ? ptcSeed : [null]).forEach(addPtcRow);

    $('#btnAddImg')?.addEventListener('click', () => addImgRow(''));
    $('#btnAddPtc')?.addEventListener('click', () => addPtcRow(null));
    document.addEventListener('click', e => {
        if (e.target.matches('[data-remove]')) e.target.closest('[data-img-row],[data-ptc-row]')?.remove();
    });

    form?.addEventListener('submit', () => {
        hiddenBag.replaceChildren();

        const urls = $$('#imgs [data-img-row] input').map(i => i.value.trim()).filter(v => v.length);
        urls.forEach((u, i) => {
            const h = document.createElement('input');
            h.type = 'hidden'; h.name = `Imagenes[${i}]`; h.value = u;
            hiddenBag.appendChild(h);
        });

        let k = 0;
        $$('#ptcs [data-ptc-row]').forEach(row => {
            const talla = row.querySelector('select[name="talla"]')?.value;
            const color = row.querySelector('select[name="color"]')?.value;
            const stock = row.querySelector('input[name="stock"]')?.value;
            if (talla && color && stock !== '') {
                [['TallaID', talla], ['ColorID', color], ['Stock', stock]].forEach(([key, val]) => {
                    const h = document.createElement('input');
                    h.type = 'hidden'; h.name = `PTCs[${k}].${key}`; h.value = val;
                    hiddenBag.appendChild(h);
                });
                k++;
            }
        });
    });
})();
