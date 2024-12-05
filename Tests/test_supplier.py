import requests
import test_helper

url = "http://localhost:3000/api/v2/suppliers/"
headers = {'APIKEY': "Admin"}
current_id = 0


def test_post_suppliers():
    response = requests.post(url, headers=headers, json=test_helper.supplier_1)
    
    assert response.status_code == 201
    global current_id
    current_id = int(response.text)
    

def test_get_after_post_suppliers():
    response = requests.get(url + str(current_id), headers=headers)

    assert response.status_code == 200

    supplier_json = response.json()
    del supplier_json['created_at']
    del supplier_json['updated_at']

    test_helper.supplier_1['id'] = current_id
    assert test_helper.supplier_1 == supplier_json


def test_get_all_suppliers():
    response = requests.get(url[:-1], headers=headers)

    assert response.status_code == 200
    
    suppliers_json = response.json()

    assert type(suppliers_json) is list
    assert 0 <= len(suppliers_json) <= 100
    if len(suppliers_json) > 0:
        assert test_helper.have_same_structure(test_helper.template_supplier, suppliers_json[0])


def test_put():
    test_helper.supplier_1_updated['id'] = current_id
    response = requests.put(url + str(current_id), headers=headers, json=test_helper.supplier_1_updated)

    assert response.status_code == 200


def test_get_after_put():
    response = requests.get(url + str(current_id), headers=headers)

    assert response.status_code == 200

    supplier_json = response.json()
    del supplier_json['updated_at']
    del supplier_json['created_at']

    test_helper.supplier_1_updated['id'] = current_id
    assert test_helper.supplier_1_updated == supplier_json


def test_delete():
    response = requests.delete(url + str(current_id), headers=headers)

    assert response.status_code == 200


def test_get_after_delete():
    response = requests.get(url + str(current_id), headers=headers)

    assert response.status_code == 404
    assert response.text == "Supplier doesn't exist with id: " + str(current_id)


