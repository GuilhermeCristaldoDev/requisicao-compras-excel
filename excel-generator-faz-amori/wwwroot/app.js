const API_URL = "https://requisicao-compras-excel-production.up.railway.app/api/purchase-requests/excel";

const itemsContainer = document.getElementById("items");
const addItemBtn = document.getElementById("addItem");
const submitBtn = document.getElementById("submit");

function createItemRow() {
    const row = document.createElement("div");
    row.className = "item-row";

    row.innerHTML = `
        <input placeholder="Tipo" class="tipo" />
        <input placeholder="Qtd" type="number" class="qtd" />
        <input placeholder="Descrição" class="descricao" />
        <input placeholder="Valor" type="number" class="valor" />
        <button class="remove">✕</button>
    `;

    row.querySelector(".remove").addEventListener("click", () => row.remove());

    return row;
}

// item inicial
itemsContainer.appendChild(createItemRow());

addItemBtn.addEventListener("click", () => {
    itemsContainer.appendChild(createItemRow());
});

submitBtn.addEventListener("click", async () => {
    const payload = {
        numeroRequisicao: document.getElementById("numeroRequisicao").value,
        solicitante: document.getElementById("solicitante").value,
        area: document.getElementById("area").value,
        data: document.getElementById("data").value,
        dentroDoOrcado: document.getElementById("orcado").value === "true",
        prazoRecebimento: document.getElementById("prazo").value,
        fornecedor1: document.getElementById("forn1").value,
        fornecedor2: document.getElementById("forn2").value,
        fornecedor3: document.getElementById("forn3").value,
        itens: []
    };

    document.querySelectorAll(".item-row").forEach(row => {
        payload.itens.push({
            tipo: row.querySelector(".tipo").value,
            quantidade: Number(row.querySelector(".qtd").value),
            descricao: row.querySelector(".descricao").value,
            valor: Number(row.querySelector(".valor").value)
        });
    });

    try {
        const res = await fetch(API_URL, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload)
        });

        if (!res.ok) throw new Error("Erro ao gerar Excel");

        const blob = await res.blob();
        const url = window.URL.createObjectURL(blob);

        const a = document.createElement("a");
        a.href = url;
        a.download = "requisicao-compras.xlsx";
        a.click();

        window.URL.revokeObjectURL(url);
    } catch (err) {
        alert("Erro ao gerar Excel");
        console.error(err);
    }
});
