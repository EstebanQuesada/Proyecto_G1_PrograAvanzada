(function () {
    function byId(id) { return document.getElementById(id); }

    const lookups = window.__LOOKUPS__ || {
        Categorias: [], Marcas: [], Proveedores: [], Tallas: [], Colores: []
    };
    const seed = window.__SEED__ || { imagenes: [], ptcs: [] };

    const categoriaSel = byId('CategoriaID');
    const marcaSel = byId('MarcaID');
    const proveedorSel = byId('ProveedorID');

    const btnAddImg = byId('btnAddImg');
    const btnAddPtc = byId('btnAddPtc');
    const imgList = byId('imgs');   
    const ptcList = byId('ptcs'); 
    let imagenes = Array.isArray(seed.imagenes) ? seed.imagenes.slice() : [];
    let ptcs = Array.isArray(seed.ptcs) ? seed.ptcs.slice() : [];

    function setOptions(selectEl, items, selected) {
        if (!selectEl) return;
        selectEl.innerHTML = '<option value="">-- Seleccione --</option>';
        items.forEach(it => {
            const id = it.id ?? it.Id;
            const nom = it.nombre ?? it.Nombre;
            const opt = document.createElement('option');
            opt.value = id;
            opt.textContent = nom;
            if (String(selected ?? '') === String(id)) opt.selected = true;
            selectEl.appendChild(opt);
        });
    }

    function renderCombosIniciales() {
        const selectedCategoria = categoriaSel ? categoriaSel.getAttribute('data-selected') ?? categoriaSel.value : '';
        const selectedMarca = marcaSel ? marcaSel.getAttribute('data-selected') ?? marcaSel.value : '';
        const selectedProv = proveedorSel ? proveedorSel.getAttribute('data-selected') ?? proveedorSel.value : '';

        setOptions(categoriaSel, lookups.Categorias, selectedCategoria);
        setOptions(marcaSel, lookups.Marcas, selectedMarca);
        setOptions(proveedorSel, lookups.Proveedores, selectedProv);
    }

    function renderImagenes() {
        if (!imgList) return;
        imgList.innerHTML = '';
        imagenes.forEach((url, i) => {
            const row = document.createElement('div');
            row.className = 'input-group mb-2';
            row.setAttribute('data-img-row', '');
            row.innerHTML = `
        <input type="text" class="form-control" value="${url ?? ''}">
        <button type="button" class="btn btn-outline-danger">Quitar</button>
      `;
            row.querySelector('button').addEventListener('click', () => {
                imagenes.splice(i, 1);
                renderImagenes();
            });
            imgList.appendChild(row);
        });
    }

    function renderPTCs() {
        if (!ptcList) return;
        ptcList.innerHTML = '';
        ptcs.forEach((p, i) => {
            const row = document.createElement('div');
            row.className = 'row g-2 align-items-end mb-2';
            row.setAttribute('data-ptc-row', '');
            row.innerHTML = `
        <div class="col-md-4">
          <label class="form-label">Talla</label>
          <select class="form-select" data-field="talla"></select>
        </div>
        <div class="col-md-4">
          <label class="form-label">Color</label>
          <select class="form-select" data-field="color"></select>
        </div>
        <div class="col-md-3">
          <label class="form-label">Stock</label>
          <input type="number" min="0" class="form-control" data-field="stock" value="${p.Stock ?? 0}">
        </div>
        <div class="col-md-1 d-grid">
          <button type="button" class="btn btn-outline-danger">X</button>
        </div>
      `;

            const tallaSel = row.querySelector('select[data-field="talla"]');
            const colorSel = row.querySelector('select[data-field="color"]');

            setOptions(tallaSel, lookups.Tallas, p.TallaID ?? '');
            setOptions(colorSel, lookups.Colores, p.ColorID ?? '');

            row.querySelector('button').addEventListener('click', () => {
                ptcs.splice(i, 1);
                renderPTCs();
            });

            ptcList.appendChild(row);
        });
    }

    function wireButtons() {
        if (btnAddImg) {
            btnAddImg.addEventListener('click', () => {
                imagenes.push('');
                renderImagenes();
            });
        }
        if (btnAddPtc) {
            btnAddPtc.addEventListener('click', () => {
                ptcs.push({ TallaID: '', ColorID: '', Stock: 0 });
                renderPTCs();
            });
        }
    }

    document.addEventListener('DOMContentLoaded', () => {
        renderCombosIniciales();
        renderImagenes();
        renderPTCs();
        wireButtons();

        const frm = document.getElementById('frmProducto');
        const bag = document.getElementById('hiddenBag');

        if (frm && bag) {
            frm.addEventListener('submit', () => {
                bag.innerHTML = '';

                const imgRows = frm.querySelectorAll('[data-img-row] input');
                Array.from(imgRows).forEach((inp, i) => {
                    const val = (inp.value || '').trim();
                    if (val.length > 0) {
                        const h = document.createElement('input');
                        h.type = 'hidden';
                        h.name = `Imagenes[${i}]`;
                        h.value = val;
                        bag.appendChild(h);
                    }
                });

                const ptcRows = frm.querySelectorAll('[data-ptc-row]');
                Array.from(ptcRows).forEach((row, i) => {
                    const talla = row.querySelector('select[data-field="talla"]')?.value || '';
                    const color = row.querySelector('select[data-field="color"]')?.value || '';
                    const stock = row.querySelector('input[data-field="stock"]')?.value || '0';

                    const h1 = document.createElement('input');
                    h1.type = 'hidden';
                    h1.name = `PTCs[${i}].TallaID`;
                    h1.value = talla;
                    bag.appendChild(h1);

                    const h2 = document.createElement('input');
                    h2.type = 'hidden';
                    h2.name = `PTCs[${i}].ColorID`;
                    h2.value = color;
                    bag.appendChild(h2);

                    const h3 = document.createElement('input');
                    h3.type = 'hidden';
                    h3.name = `PTCs[${i}].Stock`;
                    h3.value = stock;
                    bag.appendChild(h3);
                });
            });
        }
    });
})();
