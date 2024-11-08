import requests
import test_helper

url = "http://localhost:3000/api/v1/suppliers/"
headers = {'API_KEY': "a1b2c3d4e5"}


def test_get_random_suppliers():
    response = requests.get(url + str(10), headers=headers)
    
    assert response.status_code == 200

    print(test_helper.template_supplier, response.json())

    assert test_helper.have_same_structure(test_helper.template_supplier, response.json())


def test_get_all_suppliers():
    response = requests.get(url[:-1], headers=headers)

    assert response.status_code == 200
    
    suppliers_json = response.json()

    assert type(suppliers_json) is list
    assert len(suppliers_json) > 0
    assert test_helper.have_same_structure(test_helper.template_supplier, suppliers_json[0])


def test_post_suppliers():
    response = requests.post(url, headers=headers, json=test_helper.supplier_1)
    
    assert response.status_code == 201
    

def test_get_after_post_suppliers():
    response = requests.get(url + str(test_helper.supplier_1['id']), headers=headers)

    assert response.status_code == 200

    supplier_json = response.json()
    del supplier_json['created_at']
    del supplier_json['updated_at']

    assert test_helper.supplier_1 == supplier_json


def test_put():
    response = requests.put(url + str(test_helper.supplier_1['id']), headers=headers, json=test_helper.supplier_1_updated)

    assert response.status_code == 200


def test_get_after_put():
    response = requests.get(url + str(test_helper.supplier_1_updated['id']), headers=headers)

    assert response.status_code == 200

    supplier_json = response.json()
    del supplier_json['updated_at']

    assert test_helper.supplier_1_updated == supplier_json


def test_delete():
    response = requests.delete(url + str(test_helper.supplier_1_updated['id']), headers=headers)

    assert response.status_code == 200


def test_get_after_delete():
    response = requests.get(url + str(test_helper.supplier_1_updated['id']), headers=headers)

    assert response.status_code == 200
    assert response.content == b'null'