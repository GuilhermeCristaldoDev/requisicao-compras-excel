const API_URL = "https://requisicao-compras-excel-production.up.railway.app/api/purchase-requests/excel";

const itemsContainer = document.getElementById("items");
const addItemBtn = document.getElementById("addItem");
const submitBtn = document.getElementById("submit");

// Cria uma linha de item
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
addItemBtn.addEventListener("click", (e) => {
    e.preventDefault(); // não deixa o botão submeter form
    itemsContainer.appendChild(createItemRow());
});

// Submit do formulário
submitBtn.addEventListener("click", async (e) => {
    e.preventDefault(); // ⬅️ ISSO EVITA O POST NO NETLIFY

    const payload = {
        numeroRequisicao: document.getElementById("numeroRequisicao").value || "",
        solicitante: document.getElementById("solicitante").value || "",
        area: document.getElementById("area").value || "",
        data: document.getElementById("data").value,
        dentroDoOrcado: document.getElementById("orcado").value === "true",
        prazoRecebimento: document.getElementById("prazo").value,
        fornecedor1: document.getElementById("forn1").value || "",
        fornecedor2: document.getElementById("forn2").value || "",
        fornecedor3: document.getElementById("forn3").value || "",
        itens: []
    };

    document.querySelectorAll(".item-row").forEach(row => {
        const tipo = row.querySelector(".tipo").value;
        const qtd = Number(row.querySelector(".qtd").value);
        const desc = row.querySelector(".descricao").value;
        const valor = Number(row.querySelector(".valor").value);

        if (!tipo && !desc) return; // ignora linhas vazias

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
        alert("Erro ao gerar Excel. Verifique o console.");
        console.error(err);
    }
});
