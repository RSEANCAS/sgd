import Shared from "./shared";

const cBaseUrl = process.env.REACT_APP_API_URL_AUTENTICADOR;
const cAplicacionCodigo = process.env.REACT_APP_CODIGO;

const AutenticadorAPI = {
    ObtenerDataLoginAutenticar: (pUsuarioId, pContraseña, pSuccess, pError) => {
        let vUrl = `${cBaseUrl}/login/autenticar`;
        let vInit = {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                aplicacionCodigo: cAplicacionCodigo,
                usuarioId: pUsuarioId,
                contraseña: pContraseña
            })
        };
        // const res = await cFetchLoginAutenticar(pUsuarioId, pContraseña);
        Shared.FetchManaged(vUrl, vInit, "json", pSuccess, pError);
    }
}

export default AutenticadorAPI;