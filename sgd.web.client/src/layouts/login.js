import React, { useEffect } from "react";
import { Button, Card, CardBody, Col, Form, FormGroup, Row } from "reactstrap";
import { useForm } from "react-hook-form";

import "assets/scss/login.scss";
import * as AppActions from "actions/index";
import AutenticadorAPI from "api/autenticadorAPI";
import { useNavigate } from "react-router-dom";
import { useSelector, useDispatch } from "react-redux";

const keyToken = process.env.REACT_APP_LS_KEY_SGDWEBCLIENTJWT;

function LoginLayout(props) {
    const { register, handleSubmit, formState: { errors } } = useForm();
    const navigate = useNavigate();
    const { estaLogueado } = useSelector(state => state.app);
    const dispatch = useDispatch();

    const registerUsuario = register('usuario', {
        required: {
            value: true,
            message: "Debe ingresar usuario"
        },
        maxLength: {
            value: 20,
            message: "Debe ingresar máximo 20 caracteres"
        }
    });

    const registerContraseña = register('contraseña', {
        required: {
            value: true,
            message: "Debe ingresar constraseña"
        }
    });

    const onSubmit = (data) => {
        const { usuario, contraseña } = data;
        AutenticadorAPI.ObtenerDataLoginAutenticar(usuario, contraseña, onSubmitSuccess, onSubmitError);
    }

    const onSubmitSuccess = (data) => {
        if (data == null) return;
        const { success } = data;
        if (success !== true) return;
        const { token } = data;
        localStorage[keyToken] = token;
        dispatch(AppActions.setEstaLogueado(true));
        navigate("/app");
    }

    const onSubmitError = (data) => {
        console.log(data);
    }

    useEffect(() => {
        // Checking if user is not loggedIn
        if (estaLogueado === true) {
            navigate("/app");
        }
    }, [navigate, estaLogueado]);

    return (
        <>
            <div className="login-content">
                <Form className="login-form" onSubmit={handleSubmit(onSubmit)} autoComplete="off">
                    <Row style={{ marginLeft: '0px', marginRight: '0px' }}>
                        <Col xl={{ size: 4, offset: 4 }} lg={{ size: 5, offset: 3.5 }} md={{ size: 6, offset: 3 }} sm={{ size: 7, offset: 2.5 }}>
                            <Card className="card-user">
                                <div className="image">
                                    <img alt="..." src={require("assets/img/damir-bosnjak.jpg")} />
                                </div>
                                <CardBody>
                                    <div className="author">
                                        <div>
                                            <img
                                                alt="..."
                                                className="avatar border-gray"
                                                src={require("assets/img/logo/casa.png")}
                                            />
                                            <h5 className="title">Club Atlético Siempre Amigos</h5>
                                        </div>
                                        <p className="description">Ingrese sus credenciales</p>
                                    </div>

                                    <Row>
                                        <Col className="px-1" md="12">
                                            <FormGroup>
                                                <label htmlFor="txtUsuario">Usuario</label>
                                                <input id="txtUsuario" placeholder="Usuario" type="text" className={`form-control ${errors.usuario != null ? "is-invalid" : ""}`} {...registerUsuario} />
                                                {errors.usuario && <div className="invalid-feedback">{errors.usuario.message}</div>}
                                            </FormGroup>
                                        </Col>
                                    </Row>
                                    <Row>
                                        <Col className="px-1" md="12">
                                            <FormGroup>
                                                <label htmlFor="txtContraseña">Contraseña</label>
                                                <input id="txtContraseña" placeholder="Contraseña" type="password" className={`form-control ${errors.contraseña != null ? "is-invalid" : ""}`} {...registerContraseña} />
                                                {errors.contraseña && <div className="invalid-feedback">{errors.contraseña.message}</div>}
                                            </FormGroup>
                                        </Col>
                                    </Row>
                                    <Row>
                                        <div className="update ml-auto mr-auto">
                                            <Button
                                                className="btn-round"
                                                color="primary"
                                                type="submit"
                                            >
                                                Ingresar
                                            </Button>
                                        </div>
                                    </Row>
                                </CardBody>
                            </Card>
                        </Col>
                    </Row>
                </Form>
            </div>
        </>
    );
}

export default LoginLayout;
