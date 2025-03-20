const express = require("express");
const cors = require("cors");
const { AH } = require("albert-heijn-wrapper");
const { Jumbo } = require("jumbo-wrapper");
const { Aldi } = require("aldi-wrapper");
const { Coop } = require("coop-wrapper");

const app = express();
const port = process.env.PORT || 3001;

app.use(cors());
app.use(express.json());

const ah = new AH();
const jumbo = new Jumbo();
const aldi = new Aldi({ verbose: true });
const coop = new Coop();

// Albert Heijn search
app.get("/api/ah/search", async (req, res) => {
  const query = req.query.q;
  if (!query)
    return res.status(400).json({ error: "Query parameter `q` is required" });
  try {
    const results = await ah.product.search(query);
    res.json(results);
  } catch (error) {
    res.status(500).json({ error: "AH Internal Server Error" });
  }
});

// Jumbo search
app.get("/api/jumbo/search", async (req, res) => {
  const query = req.query.q;
  if (!query)
    return res.status(400).json({ error: "Query parameter `q` is required" });
  try {
    const results = await jumbo.product().getProductsFromName(query);
    res.json(results);
  } catch (error) {
    res.status(500).json({ error: "Jumbo Internal Server Error" });
  }
});

// ALDI search
app.get("/api/aldi/search", async (req, res) => {
  const query = req.query.q;
  if (!query)
    return res.status(400).json({ error: "Query parameter 'q' is required" });
  try {
    const products = await aldi.product().getProductsFromName(query);
    res.json(products.articles);
  } catch (error) {
    res.status(500).json({ error: "ALDI Internal Server Error" });
  }
});

// Coop search
// Coop search with debugging
app.get("/api/coop/search", async (req, res) => {
  const query = req.query.q;
  if (!query)
    return res.status(400).json({ error: "Query parameter 'q' is required" });

  try {
    const products = await coop.product().getProductsFromName(query);
    res.json(products);
  } catch (error) {
    console.error("Coop API Error:", error); // logs actual error to console
    res
      .status(500)
      .json({ error: error.message || "Coop Internal Server Error" });
  }
});

app.listen(port, () => {
  console.log(`Combined Supermarket API running at http://localhost:${port}`);
});
