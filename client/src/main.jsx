import { StrictMode, useEffect, useState } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter, Routes, Route} from "react-router";

createRoot(document.getElementById('root')).render(
  <StrictMode>
      <BrowserRouter>
          <Routes>
              <Route path="/kontaktlista" element={<FetchKontaktlista/>} />
              <Route path="/add-kontakt" element={<PostKontakt/>} />
          </Routes>
      </BrowserRouter>
  </StrictMode>,
)

function FetchKontaktlista() {
    const [kontakt, setKontakt] = useState([]);
    
    useEffect(() => {
        fetch("/api/kontaktlista")
            .then(response => response.json())
            .then(data => setKontakt(data));
        }
    )
}

function PostKontakt(kontakt) {
    const response = 
        fetch("/api/nyKontakt", {
            method: "POST",
            headers: {"Content-Type": "application/json"},
            body: JSON.stringify({
                namn: kontakt.namn,
                nummer: kontakt.nummer,     
            })
    })
}
