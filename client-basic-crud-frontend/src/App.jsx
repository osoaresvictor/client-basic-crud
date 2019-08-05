import React, { Component } from 'react';
import {
  Table,
  Button,
  Modal,
  ModalHeader,
  ModalBody,
  FormGroup,
  Label,
  Input,
  ModalFooter,
} from 'reactstrap';
import axios from 'axios';
import DatePicker from 'react-datepicker';

export default class App extends Component {
  constructor() {
    super();

    this.state = {
      clients: [],
      clientChangesModal: false,
      tmpClientData: App.NEW_CLIENT_DATA,
    };
  }

  static get NEW_CLIENT_DATA() {
    return {
      id: '',
      name: '',
      birthdate: '',
      registrationDate: '',
    };
  }

  componentWillMount = () => {
    this.refreshClientsList();
  }

  refreshClientsList = () => {
    axios.get('http://localhost:8000/api/clients')
      .then(response => this.setState({ clients: response.data }));
  }

  toogleClientChangesModal = () => {
    this.setState(prevState => ({ clientChangesModal: !prevState.clientChangesModal }));
  }

  clearClientDataTemp = () => {
    this.setState({ tmpClientData: App.NEW_CLIENT_DATA });
  }

  openAddClientModal = () => {
    this.clearClientDataTemp(this);
    this.setState({ operation: 'add' });
    this.toogleClientChangesModal();
  }

  openEditClientModal = (client) => {
    this.setState(
      { operation: 'update', tmpClientData: { ...client, birthdate: new Date(client.birthdate) } },
    );
    this.toogleClientChangesModal();
  }

  addClient = async () => {
    await this.setState(prevState => ({
      tmpClientData: {
        ...prevState.tmpClientData,
        registrationDate: new Date(),
      },
    }));

    const { tmpClientData } = this.state;
    axios.post('http://localhost:8000/api/clients', tmpClientData)
      .then((response) => {
        const { clients } = this.state;
        clients.push(response.data);
        this.setState({ clients });

        this.toogleClientChangesModal();
      });
  }

  updateClient = (id) => {
    const { tmpClientData } = this.state;
    axios.put(`http://localhost:8000/api/clients/${id}`, tmpClientData)
      .then(() => {
        this.clearClientDataTemp();
        this.toogleClientChangesModal();
      });
  }

  deleteClient = (id) => {
    axios.delete(`http://localhost:8000/api/clients/${id}`).then(this.refreshClientsList);
  }

  render = () => {
    const {
      clientChangesModal,
      operation,
      clients,
      tmpClientData,
    } = this.state;

    const clientsItems = clients.map(client => (
      <tr key={client.id}>
        <td>{client.id}</td>
        <td>{client.name}</td>
        <td>{new Date(client.birthdate).toLocaleDateString()}</td>
        <td>{new Date(client.registrationDate).toLocaleDateString()}</td>
        <td>
          <Button
            color="success"
            size="sm"
            className="mr-2"
            onClick={() => this.openEditClientModal(client)}
          >
            Editar
          </Button>
          <Button
            color="danger"
            size="sm"
            onClick={() => this.deleteClient(client.id)}
          >
            Excluir
          </Button>
        </td>
      </tr>
    ));

    return (
      <div className="App container">
        <h1 className="mt-2">Cadastro de Clientes</h1>

        <Button
          color="primary"
          className="my-3"
          onClick={this.openAddClientModal}
        >
          Novo Cliente
        </Button>

        <Modal
          isOpen={clientChangesModal}
          toggle={this.toogleClientChangesModal}
        >
          <ModalHeader
            toggle={this.toogleClientChangesModal}
          >
            {`${operation === 'add' ? 'Adicionar' : 'Editar'} cliente`}
          </ModalHeader>

          <ModalBody>
            <FormGroup>
              <Label for="name">Nome</Label>
              <Input
                id="name"
                value={tmpClientData.name}
                onChange={(e) => {
                  tmpClientData.name = e.target.value;
                  this.setState({ tmpClientData });
                }}
              />
            </FormGroup>
            <FormGroup>
              <Label for="birthdate">Data de Nascimento</Label>
              <br />
              <DatePicker
                id="birthdate"
                autoComplete="off"
                className="form-control"
                placeholderText="Selecione a data"
                selected={tmpClientData.birthdate}
                showMonthDropdown
                showYearDropdown
                onChange={(e) => {
                  tmpClientData.birthdate = new Date(e);
                  this.setState({ tmpClientData });
                }}
                dateFormat="dd/MM/yyyy"
              />
            </FormGroup>
          </ModalBody>

          <ModalFooter>
            <Button
              color="primary"
              onClick={
                operation === 'add'
                  ? this.addClient
                  : () => this.updateClient(tmpClientData.id)
              }
            >
              OK
            </Button>
            <Button
              color="secondary"
              onClick={this.toogleClientChangesModal}
            >
              Cancelar
            </Button>
          </ModalFooter>
        </Modal>

        <Table>
          <thead>
            <tr>
              <th>Id</th>
              <th>Nome</th>
              <th>Data de Nascimento</th>
              <th>Data de Inclusão no Sistema</th>
              <th>Ações</th>
            </tr>
          </thead>
          <tbody>
            {clientsItems}
          </tbody>
        </Table>
      </div>
    );
  }
}
