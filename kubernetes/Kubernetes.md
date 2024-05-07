# Setup Dashboard

Running Kubernetes Dashboard - instructions provided by Andrew Lock
[https://andrewlock.net/running-kubernetes-and-the-dashboard-with-docker-desktop/](https://andrewlock.net/running-kubernetes-and-the-dashboard-with-docker-desktop/)

Install Dashboard v2.7
[kubernetes dashboard v2.7.0](https://github.com/kubernetes/dashboard/releases/tag/v2.7.0)

```powershell
kubectl apply -f https://raw.githubusercontent.com/kubernetes/dashboard/v2.7.0/aio/deploy/recommended.yaml
```

Start the Kubernetes proxy:

```powershell
kubectl proxy
```

Access the Dashboard by opening the following URL in your web browser:

```text
http://localhost:8001/api/v1/namespaces/kubernetes-dashboard/services/https:kubernetes-dashboard:/proxy/
```

Setup Login User w/token

Create a user to log into Kubernetes Dashboard > https://kubernetes.io/docs/tasks/access-application-cluster/web-ui-dashboard/

Instructions for creating a sample user
https://github.com/kubernetes/dashboard/blob/master/docs/user/access-control/creating-sample-user.md

```powershell
# create dashboard-adminuser.yaml
kubectl apply -f dashboard-adminuser.yaml
serviceaccount/admin-user created

# create dashboard-cluster-admin-role.yaml
kubectl apply -f dashboard-cluster-admin-role.yaml
clusterrolebinding.rbac.authorization.k8s.io/admin-user created
```

Get the Bearer Token

```powershell
kubectl -n kubernetes-dashboard create token admin-user
```

Copy token and use it to log into kubernetes-dashboard

## Alternative - Create Secret

The Service Account and Cluster Role Binding created above can be used to create a long lived secret. This will create a token with the secret which bound the service account and the token will be saved in the Secret:

```yaml
apiVersion: v1
kind: Secret
metadata:
  name: admin-user
  namespace: kubernetes-dashboard
  annotations:
    kubernetes.io/service-account.name: 'admin-user'
type: kubernetes.io/service-account-token
```

```powershell
# create secret-adminuser.yaml
kubectl apply -f secret-adminuser.yaml
```

After Secret is created, we can execute the following command to get the token which saved in the Secret:

```shell
kubectl get secret admin-user -n kubernetes-dashboard -o jsonpath={".data.token"} | base64 -d
```

Get the Service Account Token
You need a token to log in to the dashboard. You can get the token of the service account you just created by running the following commands:

```powershell
kubectl get secrets
```

Find the dashboard-admin-sa-token (the name may vary slightly) and describe it to get the token:

```powershell
kubectl describe secret dashboard-admin-sa-token-xxxxx

kubectl describe secret -n kube-system
```

Replace xxxxx with the actual token name from the previous command. The token you need to use to log in to the dashboard is listed as token: in the output of this command.

Log in to the Dashboard
Copy the token you got from step 3, paste it into the Token field on the Kubernetes dashboard login page, then click SIGN IN.

Please note that this process gives the dashboard full administrative permissions, which can be a security risk. In a production environment, you should limit its permissions as much as possible.
