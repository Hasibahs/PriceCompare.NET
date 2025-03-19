const express = require("express");
const { Aldi } = require("aldi-wrapper");
const cors = require("cors");

const app = express();
app.use(cors());
app.use(express.json());

const aldi = new Aldi({ verbose: true });

app.get("/search", async (req, res) => {
  const query = req.query.q;
  if (!query) {
    return res.status(400).json({ error: "Query parameter 'q' is required" });
  }

  try {
    const products = await aldi.product().getProductsFromName(query);
    res.json(products.articles);
  } catch (error) {
    res.status(500).json({ error: "Failed to fetch products" });
  }
});

const PORT = 3000;
app.listen(PORT, () =>
  console.log(`Server running on http://localhost:${PORT}`)
);
