const API_URL = "https://SUA-API-URL-AQUI/api/purchase-requests/excel";

const itemsContainer = document.getElementById("items-container");
const addItemBtn = document.getElementById("add-item-btn");
const form = document.getElementById("purchase-form");

function createItemRow() {
    const row = document.createElement("div");
    row.className = "item-row";

    row.innerHTML = `
    <input type="text" class="input tipo" placeholder="Tipo" required />
    <input type="number" class="input qtd" placeholder="Qtd" min="1" value="1" required />
    <input type="text" class="input descricao" placeholder="Descrição" required />
    <input type="number" class="input valor" placeholder="Valor" min="0" step="0.01" required />
    <button type="button" class="btn remove">×</button>
  `;

    row.querySelector(".remove").addEventListener("click", () => {
        row.remove();
    });

    return row;
}

addItemBtn.addEventListener("click", () => {
    itemsContainer.appendChild(createItemRow());
});

form.addEventListener("submit", async (e) => {
    e.preventDefault();

    const payload = {
        numeroRequisicao: document.getElementById("numeroRequisicao").value,
        solicitante: document.getElementById("solicitante").value,
        area: document.getElementById("area").value,
        data: document.getElementById("data").value,
        dentroDoOrcado: document.getElementById("orcado").value === "true",
        prazoRecebimento: document.getElementById("prazo").value,
        fornecedor1: document.getElementById("fornecedor1").value,
        fornecedor2: document.getElementById("fornecedor2").value,
        fornecedor3: document.getElementById("fornecedor3").value,
        itens: []
    };

    document.querySelectorAll(".item-row").forEach(row => {
        payload.itens.push({
            tipo: row.querySelector(".tipo").value,
            quantidade: parseInt(row.querySelector(".qtd").value, 10),
            descricao: row.querySelector(".descricao").value,
            valor: parseFloat(row.querySelector(".valor").value)
        });
    });

    try {
        const response = await fetch(API_URL, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload)
        });

        if (!response.ok) {
            throw new Error("Erro ao gerar Excel");
        }

        const blob = await response.blob();
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement("a");
        a.href = url;
        a.download = "requisicao-compras.xlsx";
        document.body.appendChild(a);
        a.click();
        a.remove();
        window.URL.revokeObjectURL(url);

    } catch (err) {
        alert("Erro ao gerar o Excel. Verifique a API.");
        console.error(err);
    }
});
