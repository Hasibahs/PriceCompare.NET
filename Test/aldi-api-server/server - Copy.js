const express = require("express");
const axios = require("axios");
const cors = require("cors");

const app = express();
app.use(cors());
app.use(express.json());

const DEFAULT_PORT = 3000;

app.get("/search", async (req, res) => {
  const query = req.query.q;
  if (!query) {
    return res.status(400).json({ error: "Query parameter 'q' is required" });
  }

  console.log("Incoming query:", query);

  try {
    const ahResponse = await axios.get(
      "https://www.ah.nl/zoek/api/products/search",
      {
        params: { query, size: 10 },
        headers: {
          "User-Agent":
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36",
          Accept: "application/json, text/plain, */*",
          Referer: "https://www.ah.nl/",
          "X-Requested-With": "XMLHttpRequest",
        },
        withCredentials: true, // Just in case AH needs cookie handling
      }
    );

    res.json(ahResponse.data.products || []);
  } catch (error) {
    console.error("AH API ERROR:");

    if (error.response) {
      console.error("Status:", error.response.status);
      console.error("Headers:", error.response.headers);
      console.error("Data:", error.response.data);
    } else {
      console.error("Error Message:", error.message);
    }

    res.status(500).json({ error: "Failed to fetch products from AH" });
  }
});

// Port fallback logic
function startServer(port) {
  const server = app.listen(port, () => {
    console.log(`Albert Heijn Proxy running on http://localhost:${port}`);
  });

  server.on("error", (err) => {
    if (err.code === "EADDRINUSE") {
      console.log(`Port ${port} in use, trying ${port + 1}...`);
      startServer(port + 1);
    } else {
      console.error("Server failed to start:", err);
    }
  });
}

startServer(DEFAULT_PORT);
