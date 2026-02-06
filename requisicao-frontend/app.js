const API_URL = "https://requisicao-compras-excel-production.up.railway.app/api/purchase-requests/excel";

const itemsContainer = document.getElementById("items-container");
const addItemBtn = document.getElementById("add-item-btn");
const submitBtn = document.getElementById("submit");

function createItemRow() {
    const row = document.createElement("div");
    row.className = "item-row";

    row.innerHTML = `
        <input placeholder="Tipo" class="tipo" />
        <input placeholder="Qtd" type="number" class="qtd" />
        <input placeholder="Descrição" class="descricao" />
        <input placeholder="Valor" type="number" class="valor" />
        <button type="button" class="remove">✕</button>
    `;

    row.querySelector(".remove").addEventListener("click", () => row.remove());

    return row;
}

// Item inicial
itemsContainer.appendChild(createItemRow());

// Adicionar item
addItemBtn.addEventListener("click", () => {
    itemsContainer.appendChild(createItemRow());
});

// Submit
submitBtn.addEventListener("click", async () => {
    const payload = {
        numeroRequisicao: document.getElementById("numeroRequisicao").value || "",
        solicitante: document.getElementById("solicitante").value || "",
        area: document.getElementById("area").value || "",
        data: document.getElementById("data").value,
        dentroDoOrcado: document.getElementById("orcado").value === "true",
        prazoRecebimento: document.getElementById("prazo").value,
        fornecedor1: document.getElementById("fornecedor1").value || "",
        fornecedor2: document.getElementById("fornecedor2").value || "",
        fornecedor3: document.getElementById("fornecedor3").value || "",
        itens: []
    };

    document.querySelectorAll(".item-row").forEach(row => {
        const tipo = row.querySelector(".tipo").value;
        const qtd = Number(row.querySelector(".qtd").value);
        const desc = row.querySelector(".descricao").value;
        const valor = Number(row.querySelector(".valor").value);

        if (!tipo && !desc) return;

        payload.itens.push({
            tipo,
            quantidade: qtd,
            descricao: desc,
            valor
        });
    });

    try {
        const res = await fetch(API_URL, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload)
        });

        if (!res.ok) {
            const text = await res.text();
            console.error("Erro da API:", text);
            throw new Error("Erro ao gerar Excel");
        }

        const blob = await res.blob();
        const url = window.URL.createObjectURL(blob);

        const a = document.createElement("a");
        a.href = url;
        a.download = "requisicao-compras.xlsx";
        document.body.appendChild(a);
        a.click();

        window.URL.revokeObjectURL(url);
        a.remove();
    } catch (err) {
        alert("Erro ao gerar Excel. Veja o console.");
        console.error(err);
    }
});
