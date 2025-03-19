const express = require("express");
const cors = require("cors");
const { AH } = require("albert-heijn-wrapper");
const { Jumbo } = require("jumbo-wrapper");

const app = express();
const port = process.env.PORT || 3001;

app.use(cors());

const ah = new AH();
const jumbo = new Jumbo();

// Albert Heijn product search endpoint
app.get("/api/ah/search", async (req, res) => {
  const query = req.query.q;

  if (!query) {
    return res.status(400).json({ error: "Query parameter `q` is required" });
  }

  try {
    const results = await ah.product.search(query);
    res.json(results);
  } catch (error) {
    console.error(error);
    res.status(500).json({ error: "Internal Server Error" });
  }
});

// Jumbo product search endpoint
app.get("/api/jumbo/search", async (req, res) => {
  const query = req.query.q;

  if (!query) {
    return res.status(400).json({ error: "Query parameter `q` is required" });
  }

  try {
    const results = await jumbo.product().getProductsFromName(query);
    res.json(results);
  } catch (error) {
    console.error(error);
    res.status(500).json({ error: "Internal Server Error" });
  }
});

app.listen(port, () => {
  console.log(`Product search API running at http://localhost:${port}`);
});
